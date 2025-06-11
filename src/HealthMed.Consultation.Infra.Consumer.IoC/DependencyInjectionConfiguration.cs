using HealthMed.Consultation.Application.Consumers;
using HealthMed.Consultation.Domain.Interfaces;
using HealthMed.Consultation.Infra.Data.Context;
using HealthMed.Consultation.Infra.Data.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Polly;


namespace HealthMed.Consultation.Infra.Consumer.IoC;

public static class DependencyInjectionConfiguration
{
    public static void AddInjections(this IServiceCollection services, IConfiguration configuration)
    {
        //Data
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("postgres")));

        //Repo
        services.AddScoped<IConsultaRepository, ConsultaRepository>();

        const string serviceName = "InfraConsumerConsultation";

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
                cfg.ReceiveEndpoint("agendar-consultation-queue", e =>
                {
                    e.ConfigureConsumer<AgendarConsultaConsumer>(context);
                });

                // Configuração de fila específica para UpdateContactConsumer
                cfg.ReceiveEndpoint("atualizar-consultation-queue", e =>
                {
                    e.ConfigureConsumer<AtualizarStatusConsumer>(context);
                });

                 

                cfg.ConfigureEndpoints(context);
            });

            x.AddConsumer<AgendarConsultaConsumer>();
            x.AddConsumer<AtualizarStatusConsumer>();
     
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


