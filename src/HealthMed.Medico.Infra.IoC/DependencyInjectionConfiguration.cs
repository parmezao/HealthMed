using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using HealthMed.Doctor.Domain.Interfaces;
using HealthMed.Doctor.Infra.Data.Repositories;
using HealthMed.Doctor.Application.Interfaces;
using HealthMed.Doctor.Application.UseCases;
using Microsoft.EntityFrameworkCore;
 
using HealthMed.Doctor.Infra.Data.Context;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace HealthMed.Doctor.Infra.IoC
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddInjections(this IServiceCollection services, IConfiguration configuration)
        {
            //Data
            services.AddDbContext<DataContext>(options => options
                .UseNpgsql(configuration.GetConnectionString("postgres")));

            //Repo
            services.AddScoped<IMedicoRepository, MedicoRepository>();
            services.AddScoped<IEspecialidadeRepository, EspecialidadeRepository>();
            //Services
            services.AddScoped<IUpdateMedicoUseCase,  UpdateMedicoUseCase>();
            services.AddScoped<IInsertMedicoUseCase,  InsertMedicoUseCase>();
            services.AddScoped<IDeleteMedicoUseCase, DeleteMedicoUseCase>();
            services.AddScoped<IGetMedicosUseCase, GetMedicosUseCase>();
            services.AddScoped<IGetEspecialidadeUseCase, GetEspecialidadeUseCase>();
            services.AddScoped<IMedicoPublisher, MedicoPublisher>();


            const string serviceName = "DoctorAPI";
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
