using HealthMed.Patient.Infra.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HealthMed.Patient.Infra.Consumer.IoC;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddInjections(builder.Configuration);

var host = builder.Build();
host.Run();
