using HealthMed.Patient.Infra.IoC;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
   .AddJsonFile("appsettings.json", optional: true)
   .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
   .AddEnvironmentVariables();

// Controllers
builder.Services.AddControllers();

// Swagger com metadados e suporte a JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HealthMed.Patient.API",
        Version = "v1",
        Description = "API para gerenciamento de pacientes no sistema HealthMed"
    });

    // Suporte a autenticação via JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Insira o token no formato: Bearer {seu_token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Injeções (IoC) para Repos, UseCases, JWT, MassTransit, OpenTelemetry

builder.Services.AddInjections(builder.Configuration);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();


var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HealthMed.Patient.API v1");
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // mostra detalhes no navegador
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Prometheus
app.MapPrometheusScrapingEndpoint();

// HTTPS e Auth
app.UseHttpsRedirection();

app.UseAuthentication(); // ⚠️ Sempre antes do Authorization
app.UseAuthorization();

// Rotas
app.MapControllers();
// Mapear os Endpoints de Health Checks
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new { e.Key, Status = e.Value.Status.ToString() })
        });
        await context.Response.WriteAsync(result);
    }
});

app.Run();

public partial class Program;