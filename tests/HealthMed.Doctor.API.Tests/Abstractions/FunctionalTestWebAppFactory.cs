using HealthMed.Doctor.Infra.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Images;
using Testcontainers.PostgreSql;
using Microsoft.Extensions.Configuration;
namespace HealthMed.Doctor.API.Tests.Abstractions
{
   
        public class FunctionalTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
        {
            private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();
            private readonly IContainer _rabbitMqContainer = new ContainerBuilder()
               .WithImage("rabbitmq:management")
               .WithEnvironment("RABBITMQ_DEFAULT_USER", "guest")
               .WithEnvironment("RABBITMQ_DEFAULT_PASS", "guest")
               .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
               .Build();
            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                builder.ConfigureServices(services =>
                {

                    services.Remove(services.Single(a => typeof(DbContextOptions<DataContext>) == a.ServiceType));
                    services.AddDbContext<DataContext>(options => options
                        .UseNpgsql(_postgreSqlContainer.GetConnectionString()));

                    RunScriptDatabase(services);
                });
            }

            private void RunScriptDatabase(IServiceCollection services)
            {
                var serviceProvider = services.BuildServiceProvider();
                var dataContext = serviceProvider.GetRequiredService<DataContext>();
                dataContext.Database.EnsureCreated();
            }
            public async Task InitializeAsync()
            {
                await _postgreSqlContainer.StartAsync();
                await _rabbitMqContainer.StartAsync();

                Environment.SetEnvironmentVariable("ConnectionStrings__postgres", _postgreSqlContainer.GetConnectionString());
                Environment.SetEnvironmentVariable("RABBITMQ__HOST", "localhost");
                Environment.SetEnvironmentVariable("RABBITMQ__PORT", "5672");
                Environment.SetEnvironmentVariable("RABBITMQ__USER", "guest");
                Environment.SetEnvironmentVariable("RABBITMQ__PASSWORD", "guest");
            }

            public new async Task DisposeAsync()
            {
                await _postgreSqlContainer.StopAsync();
                await _rabbitMqContainer.DisposeAsync();
            }
        }

    }
