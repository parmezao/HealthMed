using HealthMed.Doctor.Application.Consumers;
using HealthMed.Doctor.Domain.Interfaces;
using HealthMed.Doctor.Infra.Data.Context;
using HealthMed.Doctor.Infra.Data.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Polly;

namespace HealthMed.Doctor.Infra.Consumer.IoC;

public static class DependencyInjectionConfiguration
{
    public static void AddInjections(this IServiceCollection services, IConfiguration configuration)
    {
        //Data
        services.AddDbContext<DataContext>(options => options
            .UseNpgsql(configuration.GetConnectionString("postgres")));

        //Repo
        services.AddScoped<IMedicoRepository, MedicoRepository>();

        const string serviceName = "InfraConsumerDoctor";

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                //"RABBITMQ_HOST": "rabbitmq",
                //"RABBITMQ_PORT": "5672",
                //"RABBITMQ_USER": "guest",
                //"RABBITMQ_PASSWORD": "guest"
                var rabbitMqHost = "rabbitmq";// configuration["RabbitMQ:RABBITMQ_HOST"];
                var rabbitMqUser = "guest";// configuration["RabbitMQ:RABBITMQ_USER"];
                var rabbitMqPassword = "guest";// configuration["RabbitMQ:RABBITMQ_PASSWORD"];

                cfg.Host(rabbitMqHost, h =>
                {
                    h.Username(rabbitMqUser);
                    h.Password(rabbitMqPassword);
                });

                // Configuração de fila específica para InsertContactConsumer
                cfg.ReceiveEndpoint("insert-doctor-queue", e =>
                {
                    e.ConfigureConsumer<InsertMedicoConsumer>(context);
                });

                // Configuração de fila específica para UpdateContactConsumer
                cfg.ReceiveEndpoint("update-doctor-queue", e =>
                {
                    e.ConfigureConsumer<UpdateMedicoConsumer>(context);
                });

                // Configuração de fila específica para DeleteContactConsumer
                cfg.ReceiveEndpoint("delete-doctor-queue", e =>
                {
                    e.ConfigureConsumer<DeleteMedicoConsumer>(context);
                });

                cfg.ConfigureEndpoints(context);
            });

            x.AddConsumer<InsertMedicoConsumer>();
            x.AddConsumer<UpdateMedicoConsumer>();
            x.AddConsumer<DeleteMedicoConsumer>();
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


