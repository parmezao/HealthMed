using Docker.DotNet.Models;
using HealthMed.Auth.Domain.Entities;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.API.Tests.Abstractions
{
    public class BaseFunctionalTests : IClassFixture<FunctionalTestWebAppFactory>
    {
  
     protected HttpClient HttpClient { get; init; }
        private readonly IServiceProvider _serviceProvider;
        private readonly string _authUrl = "http://localhost:8081";
        public BaseFunctionalTests(FunctionalTestWebAppFactory webAppFactory)
        {
            HttpClient = webAppFactory.CreateClient();
            _serviceProvider = webAppFactory.Services;
            LoginInterno().GetAwaiter().GetResult();
        }

        private async Task LoginInterno()
        {
            using var scope = _serviceProvider.CreateScope();

            var services = new ServiceCollection();
            var configuration = ConfigureAppSettings();
                 

            //var loginUseCase = scope.ServiceProvider.GetRequiredService<ILoginUseCase>();

            var loginRequest = new LoginRequest
            {
                Login = "CRM123456",
                Senha = "123456"
            };
            HttpClient _HttpClient = new HttpClient();
             var responseToken = await _HttpClient.PostAsJsonAsync($"{_authUrl}/auth/login", loginRequest); // Retorna token JWT Bearer
            responseToken.EnsureSuccessStatusCode();
            var loginResponse = await responseToken.Content.ReadFromJsonAsync<LoginResponse>();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse!.Token);
           
        }

        private static IConfiguration ConfigureAppSettings()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

       




    }
}
