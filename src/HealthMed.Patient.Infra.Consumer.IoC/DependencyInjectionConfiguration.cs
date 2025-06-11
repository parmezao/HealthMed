using HealthMed.Patient.Application.Consumers;
using HealthMed.Patient.Domain.Interfaces;
using HealthMed.Patient.Infra.Data.Context;
using HealthMed.Patient.Infra.Data.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Polly;

namespace HealthMed.Patient.Infra.Consumer.IoC
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddInjections(this IServiceCollection services, IConfiguration configuration)
        {
            //Data
            services.AddDbContext<DataContext>(options => options
                .UseNpgsql(configuration.GetConnectionString("postgres")));

            //Repo
            services.AddScoped<IPacienteRepository, PacienteRepository>();

            const string serviceName = "InfraConsumerPacient";

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
                    cfg.ReceiveEndpoint("insert-pacient-queue", e =>
                    {
                        e.ConfigureConsumer<InsertPacienteConsumer>(context);
                    });

                    // Configuração de fila específica para UpdateContactConsumer
                    cfg.ReceiveEndpoint("update-pacient-queue", e =>
                    {
                        e.ConfigureConsumer<UpdatePacienteConsumer>(context);
                    });

                    // Configuração de fila específica para DeleteContactConsumer
                    cfg.ReceiveEndpoint("delete-pacient-queue", e =>
                    {
                        e.ConfigureConsumer<DeletePacienteConsumer>(context);
                    });

                    cfg.ConfigureEndpoints(context);
                });

                x.AddConsumer<InsertPacienteConsumer>();
                x.AddConsumer<UpdatePacienteConsumer>();
                x.AddConsumer<DeletePacienteConsumer>();
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
