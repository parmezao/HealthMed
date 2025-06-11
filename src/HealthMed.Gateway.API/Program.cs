using DeviceDetectorNET.Parser.Device;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Text;
using Yarp.ReverseProxy;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

// JWT manual (sem authority / discovery)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "HealthMed.Auth",

            ValidateAudience = true,
            ValidAudience = "HealthMed.Gateway",

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("KJNJKBJHBSHJVJHJVDHBHSLSHUBUJNOSSSOLWSSDSGVHGDJHVHGFH") // Mesma usada no Auth.API
            )
        };
    });
// Adicionar Health Checks
builder.Services.AddHealthChecks()
    .AddCheck("API Health", () =>
        Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("API está saudável"));
builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("HealthMed.Gateway.API"))
               .AddAspNetCoreInstrumentation()
               .AddPrometheusExporter();
    })
    .WithTracing(tracing =>
    {
        tracing.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("HealthMed.Gateway.API"))
               .AddAspNetCoreInstrumentation()
               .AddHttpClientInstrumentation()
               .AddOtlpExporter();
    });

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway API v1");
    options.IndexStream = () => File.OpenRead("wwwroot/swagger/index.html");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapReverseProxy();
app.MapGet("/", () => Results.Redirect("/swagger"));
app.UseOpenTelemetryPrometheusScrapingEndpoint();
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