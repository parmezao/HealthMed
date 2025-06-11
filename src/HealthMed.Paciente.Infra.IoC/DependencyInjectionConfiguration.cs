using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using HealthMed.Patient.Domain.Interfaces;
using HealthMed.Patient.Infra.Data.Repositories;
using HealthMed.Patient.Application.Interfaces;
using HealthMed.Patient.Application.UseCases;
using Microsoft.EntityFrameworkCore;
using HealthMed.Patient.Infra.Data.Context;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Text;
using HealthMed.Patient.Application.Publisher;

namespace HealthMed.Patient.Infra.IoC
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
          
            //Services
            services.AddScoped<IUpdatePacienteUseCase, UpdatePacienteUseCase>();
            services.AddScoped<IInsertPacienteUseCase, InsertPacienteUseCase>();
            services.AddScoped<IDeletePacienteUseCase, DeletePacienteUseCase>();
            services.AddScoped<IGetPacienteUseCase, GetPacienteUseCase>();
         
            services.AddScoped<IPacientePublisher, PacientePublisher>();

            const string serviceName = "PatientAPI";
            // JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration.GetValue<string>("SecretJWT"))),

                    ValidateIssuer = true,
                    ValidIssuer = "HealthMed.Auth.API",

                    ValidateAudience = true,
                    ValidAudience = "healthmed-api",

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

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

                    // Configuração de Retry
                    cfg.UseMessageRetry(retryConfig =>
                    {
                        retryConfig.Interval(3, TimeSpan.FromSeconds(5)); // Tentar 3 vezes com intervalos de 5 segundos
                    });

                    // Configuração do Circuit Breaker
                    cfg.UseCircuitBreaker(cb =>
                    {
                        cb.TrackingPeriod = TimeSpan.FromMinutes(1);  // Período para acompanhar falhas
                        cb.TripThreshold = 15;  // Número máximo de falhas para abrir o circuito
                        cb.ActiveThreshold = 10; // Número máximo de falhas ativas antes de abrir o circuito
                        cb.ResetInterval = TimeSpan.FromMinutes(2);  // Intervalo para resetar o circuito
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            // Adicionar Health Checks
            services.AddHealthChecks()
                .AddCheck("API Health", () =>
                    Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("API está saudável"));

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
