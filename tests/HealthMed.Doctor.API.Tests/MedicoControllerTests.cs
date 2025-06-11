using Bogus;
using FluentAssertions;
using HealthMed.Doctor.API.Tests.Abstractions;
using HealthMed.Doctor.Application.ViewModels;
using HealthMed.Doctor.Domain.Entities;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
    public class MedicoControllerTests
    {
        private readonly Faker _faker;
 
        private readonly string _apiDoctorUrl = "http://localhost:8083";
        private readonly string _authUrl = "http://localhost:8081";
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        public MedicoControllerTests()
        {
            _client = new HttpClient();
            _faker = new Faker(locale: "pt_BR"); 
        }

        [Fact(DisplayName = "Deve inserir um novo médico e retornar PublishResponse")]
        public async Task Insert_DeveRetornar200()
        {
            var request = new InsertMedicoRequest(

                "Dr. Teste",
                 "Cardiologia",
                 $"CRM{Guid.NewGuid().ToString("N")[..6]}",
                 new List<HorarioDto>
                {
                    new() { DataHora = DateTime.Now.AddDays(1), Ocupado = false }
                 }
            );
            await this.Login();
            var response = await _client.PostAsJsonAsync($"{_apiDoctorUrl}/api/medico/insert", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<PublishResponse>();
            result!.Message.Should().Be("Cadastro em processamento.");
        }

        [Fact(DisplayName = "Deve atualizar um médico e retornar PublishResponse")]
        public async Task Update_DeveRetornar200()
        {
            var medico = await EnsureAnyMedicoExists();
            if (medico != null)
            {
                var updateRequest = new UpdateMedicoRequest(

                     medico.Id,
                    "Dr. Atualizado",
                    medico.Especialidade,
                    medico.CRM,
                     new List<HorarioDto>
                    {
                    new() { DataHora = DateTime.Now.AddDays(2), Ocupado = false }
                    }
                );
                await this.Login();
                var response = await _client.PutAsJsonAsync($"{_apiDoctorUrl}/api/medico/update", updateRequest);

                response.StatusCode.Should().Be(HttpStatusCode.OK);
                var result = await response.Content.ReadFromJsonAsync<PublishResponse>();
                result!.Message.Should().Be("Atualização em processamento.");
            }
        }

        [Fact(DisplayName = "Deve excluir um médico e retornar PublishResponse")]
        public async Task Delete_DeveRetornar200()
        {
            var medico = await EnsureAnyMedicoExists();

            if (medico != null)
            {
                await this.Login();
                var response = await _client.DeleteAsync($"{_apiDoctorUrl}/api/medico/delete?id={medico.Id}");

                response.StatusCode.Should().Be(HttpStatusCode.OK);
                var result = await response.Content.ReadFromJsonAsync<PublishResponse>();
                result!.Message.Should().Be("Exclusão em processamento.");
            }
        }

        [Fact(DisplayName = "Deve retornar todos os médicos")]
        public async Task GetAll_DeveRetornar200()
        {
            await this.Login();
            var response = await _client.GetAsync($"{_apiDoctorUrl}/api/medico/getall");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var medicos = await response.Content.ReadFromJsonAsync<List<MedicoResponse>>();
            medicos.Should().NotBeNull();
        }

        [Fact(DisplayName = "Deve retornar todos os médicos com a especialidade filtrada")]
        public async Task GetAllPorEspecialidade_DeveRetornar200()
        {
            await this.Login();
            string especialidadeEscaped = Uri.EscapeDataString("Clínica Geral");
            var response = await _client.GetAsync($"{_apiDoctorUrl}/api/medico/getall?especialidade={especialidadeEscaped}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var medicos = await response.Content.ReadFromJsonAsync<List<MedicoResponse>>();
            medicos.Should().NotBeNull();
        }

        [Fact(DisplayName = "Não deve retornar médicos se especialidade for inválida")]
        public async Task GetAllPorEspecialidadeInvalida_DeveRetornar200_SemNenhumMedicoCorrespondente()
        {
            await this.Login();
            string especialidadeEscaped = Uri.EscapeDataString("Qualquer Especialidade");
            var response = await _client.GetAsync($"{_apiDoctorUrl}/api/medico/getall?especialidade={especialidadeEscaped}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var medicos = await response.Content.ReadFromJsonAsync<List<MedicoResponse>>();
            medicos.Should().BeEmpty();
        }

        [Fact(DisplayName = "Deve buscar médico por CRM")]
        public async Task GetByCRM_DeveRetornar200()
        {
            var medico = await EnsureAnyMedicoExists();
            //await this.Login();
            var response = await _client.GetAsync($"{_apiDoctorUrl}/api/medico/getbycrm?crm={medico.CRM}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var fetched = await response.Content.ReadFromJsonAsync<MedicoResponse>();
            fetched!.CRM.Should().Be(medico.CRM);
        }

        [Fact(DisplayName = "Deve buscar médico por ID")]
        public async Task GetById_DeveRetornar200()
        {
            var medico = await EnsureAnyMedicoExists();
            //await this.Login();
            if (medico != null)
            {
                var response = await _client.GetAsync($"{_apiDoctorUrl}/api/medico/getbyid?id={medico.Id}");

                response.StatusCode.Should().Be(HttpStatusCode.OK);
                var fetched = await response.Content.ReadFromJsonAsync<MedicoResponse>();
                fetched!.Id.Should().Be(medico.Id);
            }
        }

        private async Task<MedicoResponse> EnsureAnyMedicoExists()
        {
            // Autenticate
            await this.Login();

            //Act
            var response = await _client.GetAsync($"{_apiDoctorUrl}/api/medico/getall");
            //Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();

            var lista = JsonSerializer.Deserialize<List<MedicoResponse>>(content, jsonOptions)!;


            if (lista is not null && lista.Any())
                return lista.First();

            // Inserir novo médico se não houver nenhum
            var insertRequest = new InsertMedicoRequest(

                "Dr. AutoSeed",
                 "Cardiologia",
                $"CRM{Guid.NewGuid().ToString("N")[..6]}",
                 new List<HorarioDto>
                {
                    new() { DataHora = DateTime.Now.AddDays(1), Ocupado = false }
                }
            );

            var insertResponse = await _client.PostAsJsonAsync($"{_apiDoctorUrl}/api/medico/insert", insertRequest);
            insertResponse.EnsureSuccessStatusCode();
            System.Threading.Thread.Sleep(1000);
            var responseretry = await _client.GetAsync($"{_apiDoctorUrl}/api/medico/getall");
            responseretry.EnsureSuccessStatusCode();
            responseretry.StatusCode.Should().Be(HttpStatusCode.OK);
            var contentretry = await response.Content.ReadAsStringAsync();
            var atualizada = JsonSerializer.Deserialize<List<MedicoResponse>>(content, jsonOptions)!;        

            return atualizada.FirstOrDefault();
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