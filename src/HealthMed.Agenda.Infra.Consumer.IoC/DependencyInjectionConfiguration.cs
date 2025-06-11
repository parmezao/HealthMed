using HealthMed.Agenda.Application.Consumers;
using HealthMed.Agenda.Domain.Interfaces;
using HealthMed.Agenda.Infra.Data.Context;
using HealthMed.Agenda.Infra.Data.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Polly;

namespace HealthMed.Agenda.Infra.Consumer.IoC
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddInjections(this IServiceCollection services, IConfiguration configuration)
        {
            //Data
            services.AddDbContext<DataContext>(options => options
                .UseNpgsql(configuration.GetConnectionString("postgres")));

            //Repo
            services.AddScoped<IAgendaRepository, AgendaRepository>();

            const string serviceName = "InfraConsumerAgenda";

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {

                    var rabbitMqHost = configuration["RabbitMQ:RABBITMQ_HOST"];
                    var rabbitMqUser = configuration["RabbitMQ:RABBITMQ_USER"];
                    var rabbitMqPassword = configuration["RabbitMQ:RABBITMQ_PASSWORD"];

                    cfg.Host(rabbitMqHost, h =>
                    {
                        h.Username(rabbitMqUser);
                        h.Password(rabbitMqPassword);
                    });

                    // Configuração de fila específica para InsertContactConsumer
                    cfg.ReceiveEndpoint("insert-agenda-queue", e =>
                    {
                        e.ConfigureConsumer<CadastrarHorarioConsumer>(context);
                    });

                    // Configuração de fila específica para UpdateContactConsumer
                    cfg.ReceiveEndpoint("update-agenda-queue", e =>
                    {
                        e.ConfigureConsumer<EditarHorarioConsumer>(context);
                    });

                    // Configuração de fila específica para DeleteContactConsumer
                    cfg.ReceiveEndpoint("delete-agenda-queue", e =>
                    {
                        e.ConfigureConsumer<RemoverHorarioConsumer>(context);
                    });

                    cfg.ConfigureEndpoints(context);
                });

                x.AddConsumer<CadastrarHorarioConsumer>();
                x.AddConsumer<EditarHorarioConsumer>();
                x.AddConsumer<RemoverHorarioConsumer>();
            });



            // Configuração do OpenTelemetry
            services.AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService(serviceName))
                .WithTracing(tracing => tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                )
                .WithMetrics(metrics =>
                {
                    metrics
                        .AddRuntimeInstrumentation()
                        .AddProcessInstrumentation()
                        .AddAspNetCoreInstrumentation()
                        .AddPrometheusExporter();  // Exposição para Prometheus
                });


            // Configuração de Polly para RabbitMQ com retries
            services.AddSingleton<IAsyncPolicy>(policy =>
            {
                return Policy.Handle<Exception>()
                             .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            });




            services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
            )
            .WithMetrics(metrics =>
            {
                metrics
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddPrometheusExporter();  // Exposição para Prometheus
            });
        }
    }
}
