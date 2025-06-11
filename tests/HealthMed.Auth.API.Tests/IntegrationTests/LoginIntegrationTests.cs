using FluentAssertions;
using HealthMed.Auth.Application.ViewModels;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Net.Http;

namespace HealthMed.Auth.Integration.Tests.IntegrationTests
{
    public class LoginIntegrationTests
    {
        private readonly string _authUrl = "http://localhost:8081";
        private readonly HttpClient _client;
        public LoginIntegrationTests()
        {
            _client = new HttpClient();
        }

        [Theory(DisplayName = "O sistema deve permitir que o médico faça login usando o número de CRM e uma senha.")]
        [InlineData("CRMADMIN", "123456")]
        public async Task WhenDoctorCredentialsIsValid_ShouldReturnSuccessInLogin(string login, string password)
        {
            // Arrange
            var loginRequest = new LoginRequest { Login = login, Senha = password };
            
            // Act
            var responseToken = await _client.PostAsJsonAsync($"{_authUrl}/auth/login", loginRequest);

            // Assert
            responseToken.EnsureSuccessStatusCode();
            var loginResponse = await responseToken.Content.ReadFromJsonAsync<LoginResponse>();
            loginResponse.Should().NotBeNull();
            loginResponse.Nome.Should().NotBeNullOrEmpty();
            loginResponse.Token.Should().NotBeNullOrEmpty();
            loginResponse.Role.Should().NotBeNullOrEmpty();
        }

        [Theory(DisplayName = "O sistema não deve permitir que o médico faça login usando o número de CRM e/ou senha inválido(s).")]
        [InlineData("crminvalido", "123456")]
        [InlineData("outrocrm", "123456")]
        public async Task WhenDoctorCredentialsIsInvalid_ShouldReturnFailureInLogin(string login, string password)
        {
            // Arrange
            var loginRequest = new LoginRequest { Login = login, Senha = password };

            // Act
            var loginResponse = await _client.PostAsJsonAsync($"{_authUrl}/auth/login", loginRequest);

            // Assert
            loginResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory(DisplayName = "O sistema deve permitir que o paciente faça login usando um e-mail ou CPF e uma senha.")]
        [InlineData("maria@example.com", "123456")]
        [InlineData("12345678901", "123456")]
        public async Task WhenPatientCredentialsIsValid_ShouldReturnSuccessInLogin(string login, string password)
        {
            // Arrange
            var loginRequest = new LoginRequest { Login = login, Senha = password };

            // Act
            var responseToken = await _client.PostAsJsonAsync($"{_authUrl}/auth/login", loginRequest);

            // Assert
            responseToken.EnsureSuccessStatusCode();
            var loginResponse = await responseToken.Content.ReadFromJsonAsync<LoginResponse>();
            loginResponse.Should().NotBeNull();
            loginResponse.Nome.Should().NotBeNullOrEmpty();
            loginResponse.Token.Should().NotBeNullOrEmpty();
            loginResponse.Role.Should().NotBeNullOrEmpty();
        }

        [Theory(DisplayName = "O sistema não deve permitir que o paciente faça login usando um e-mail ou CPF e uma senha inválido(s).")]
        [InlineData("maria@example2.com", "123456")]
        [InlineData("12345678901", "852364")]
        public async Task WhenPatientCredentialsIsInvalid_ShouldReturnFailureInLogin(string login, string password)
        {
            // Arrange
            var loginRequest = new LoginRequest { Login = login, Senha = password };

            // Act
            var loginResponse = await _client.PostAsJsonAsync($"{_authUrl}/auth/login", loginRequest);

            // Assert
            loginResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
