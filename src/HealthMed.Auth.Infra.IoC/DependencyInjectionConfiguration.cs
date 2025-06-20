using HealthMed.Auth.Infra.Data.Context;
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
using HealthMed.Auth.Domain.Interfaces;
using HealthMed.Auth.Infra.Data.Repositories;
using HealthMed.Auth.Application.Interfaces;
using HealthMed.Auth.Application.UseCases;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace HealthMed.Auth.Infra.IoC
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddInjections(this IServiceCollection services, IConfiguration configuration)
        {
            //Data
            services.AddDbContext<DataContext>(options => options
                .UseNpgsql(configuration.GetConnectionString("postgres")));

            //Repo
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            //Services
            services.AddScoped<ILoginUseCase, LoginUseCase>();


            var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretJWT"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            const string serviceName = "AuthAPI";

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
