using HealthMed.Consultation.Infra.Consumer;
using HealthMed.Consultation.Infra.Consumer.IoC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
 

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddInjections(builder.Configuration);

var host = builder.Build();
host.Run();
