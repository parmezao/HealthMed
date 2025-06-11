using HealthMed.Auth.Infra.IoC;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInjections(builder.Configuration);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();


var app = builder.Build();
app.UseCors("AllowAll");

app.MapPrometheusScrapingEndpoint();

 app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});
app.UseHttpsRedirection();

app.UseAuthorization();

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