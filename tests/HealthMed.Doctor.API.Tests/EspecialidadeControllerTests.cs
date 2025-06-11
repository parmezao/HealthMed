using Bogus;
using FluentAssertions;
using HealthMed.Doctor.API.Tests.Abstractions;
using HealthMed.Doctor.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthMed.Doctor.API.Tests
{
    public class EspecialidadeControllerTests 
    {
        private readonly Faker _faker;
        private readonly FunctionalTestWebAppFactory _testsFixture;
        private readonly HttpClient _client;
        private readonly string _authUrl = "http://localhost:8081";
        private readonly string _apiDoctorUrl = "http://localhost:8083";
   
        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        public EspecialidadeControllerTests()
        {
            _client = new HttpClient();
            _faker = new Faker(locale: "pt_BR");
          
        }

        [Fact(DisplayName = "Deve retornar 200 OK ao listar todas as especialidades")]
        public async Task ListarTodasAsync_DeveRetornar200()
        {     // Autenticate
            await this.Login();
            var response = await _client.GetAsync($"{_apiDoctorUrl}/api/especialidade/listar-todas");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var especialidades = await response.Content.ReadFromJsonAsync<List<EspecialidadeResponse>>();
            especialidades.Should().NotBeNull();
        }

        [Fact(DisplayName = "Deve retornar 200 OK ao buscar especialidade por nome")]
        public async Task GetByNomeAsync_DeveRetornar200()
        {
            var nome = "Cardiologia";
            // Autenticate
            await this.Login();
            var response = await _client.GetAsync($"{_apiDoctorUrl}/api/especialidade/get-by-nome?nome={nome}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var especialidade = await response.Content.ReadFromJsonAsync<EspecialidadeResponse>();
            especialidade.Should().NotBeNull();
            especialidade!.Nome.Should().Be(nome);
        }

        [Fact(DisplayName = "Deve retornar 200 OK ao buscar especialidade por categoria")]
        public async Task GetByCategoriaAsync_DeveRetornar200()
        {
            var categoria = "Cirúrgica";
            // Autenticate
            await this.Login();
            var response = await _client.GetAsync($"{_apiDoctorUrl}/api/Especialidade/get-by-categoria?categoria={categoria}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var especialidade = await response.Content.ReadFromJsonAsync<EspecialidadeResponse>();
            especialidade.Should().NotBeNull();
            especialidade!.Categoria.Should().Be(categoria);
        }

        public async Task Login(string login = "CRMADMIN", string senha = "123456")
        {     
            var loginRequest = new LoginRequest { Login = login, Senha = senha };
            var responseToken = await _client.PostAsJsonAsync($"{_authUrl}/auth/login", loginRequest); // Retorna token JWT Bearer
            responseToken.EnsureSuccessStatusCode();
            var loginResponse = await responseToken.Content.ReadFromJsonAsync<LoginResponse>();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse!.Token);
        }
    }
}
 
