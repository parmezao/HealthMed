using HealthMed.Agenda.Infra.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HealthMed.Agenda.Infra.Consumer.IoC;
using Microsoft.Extensions.Configuration;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddInjections(builder.Configuration);

var host = builder.Build();
host.Run();
