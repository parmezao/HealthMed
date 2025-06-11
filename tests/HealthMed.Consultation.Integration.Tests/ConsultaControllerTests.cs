using FluentAssertions;
using HealthMed.Consultation.Application.ViewModels;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using static MassTransit.ValidationResultExtensions;

namespace HealthMed.Consultation.Integration.Tests
{
    public class ConsultaControllerTests
    {

        private readonly string _apiConsultaUrl = "http://localhost:8090";
        private readonly string _authUrl = "http://localhost:8081";
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        public ConsultaControllerTests()
        {
            _client = new HttpClient();

        }

        [Fact]
        public async Task AgendarConsulta_DeveRetornarSucesso()
        {
            await Login("12345678901", "123456");

            var request = new AgendarConsultaRequest
                     ("12345678900",
                        "Paciente Teste",
                          "CRM123456",
                           DateTime.UtcNow.AddDays(1),
                          "Agendada",
                          "Checkup"
                      );

            var response = await _client.PostAsJsonAsync($"{_apiConsultaUrl}/api/Consulta/agendar", request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ConsultasDoPaciente_DeveRetornarOK()
        {
            await Login("12345678901", "123456");

            var response = await _client.GetAsync($"{_apiConsultaUrl}/api/Consulta/paciente");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ConsultasDoMedico_DeveRetornarOK()
        {
            await Login();

            var response = await _client.GetAsync($"{_apiConsultaUrl}/api/Consulta/medico");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        //[Fact]
        //public async Task AceitarConsulta_DeveRetornarOK()
        //{
        //    await Login();
        //    var consulta = EnsureAnyAgendaMedicosExists();
        //    int id = 0;
        //    if (consulta != null)
        //    {
        //        id = consulta.Id;
        //    }
        //    var response = await _client.PutAsync($"{_apiConsultaUrl}/api/Consulta/{id}/aceitar", null);
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}

        //[Fact]
        //public async Task CancelarConsulta_DeveRetornarOK()
        //{
        //     // tem que incluir uma consulta caso não exista
        //    await Login("12345678901", "123456");
        //    var request = new JustificativaRequest("Motivo pessoal");


        //    var consulta = EnsureAnyAgendaPacienteExists();
        //    int id = 0;
        //    if(consulta != null)
        //    {
        //        id = consulta.Id;
        //    }
        //    var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

       
        //    var response = await _client.PutAsync($"{_apiConsultaUrl}/api/Consulta/{id}/cancelar", content);

        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}

        //[Fact]
        //public async Task RecusarConsulta_DeveRetornarOK()
        //{
        //   await Login();

        //    var request = new JustificativaRequest("Agenda cheia");

        //    var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        //    //Act
        //    var response = await _client.GetAsync($"{_apiConsultaUrl}/api/Consulta/medico");
        //    //Assert
        //    response.EnsureSuccessStatusCode();
        //    response.StatusCode.Should().Be(HttpStatusCode.OK);
        //    var contentcon = await response.Content.ReadAsStringAsync();

        //    var lista = JsonSerializer.Deserialize<List<ConsultaResponse>>(contentcon, jsonOptions)!;

        //    ConsultaResponse consulta;
        //    if (lista is not null && lista.Any())
        //    {
        //        consulta = lista.FirstOrDefault();
        //        var responseRec = await _client.PutAsync($"{_apiConsultaUrl}/api/Consulta/{consulta.Id}/recusar", content);

        //        Assert.Equal(HttpStatusCode.OK, responseRec.StatusCode);
        //    }
        //    else
        //    {
        //        await Login("12345678901", "123456");
        //        var requestAgen = new AgendarConsultaRequest
        //                 ("12345678900",
        //                    "Paciente Teste",
        //                      "CRM123456",
        //                       DateTime.UtcNow.AddDays(1),
        //                      "Pendente",
        //                      "Checkup"
        //                  );

        //        var contentÍnse = new StringContent(JsonSerializer.Serialize(requestAgen), Encoding.UTF8, "application/json");
        //        var insertResponse = await _client.PostAsJsonAsync($"{_apiConsultaUrl}/api/Consulta/agendar", contentÍnse);


        //        insertResponse.EnsureSuccessStatusCode();
        //        insertResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        //        var contentinse = await response.Content.ReadAsStringAsync();


        //        System.Threading.Thread.Sleep(50000);

        //        await Login();

        //        var responseretry = await _client.GetAsync($"{_apiConsultaUrl}/api/Consulta/medico");
        //        responseretry.EnsureSuccessStatusCode();
        //        responseretry.StatusCode.Should().Be(HttpStatusCode.OK);
        //        var contentretry = await response.Content.ReadAsStringAsync();
        //        var atualizada = JsonSerializer.Deserialize<List<ConsultaResponse>>(contentretry, jsonOptions)!;

        //        if (atualizada is not null && atualizada.Any())
        //        {

        //            consulta = atualizada.Select(x => new ConsultaResponse(x.Id, x.CpfPaciente, x.NomePaciente, x.CrmMedico, x.DataHora, x.Status, x.Justificativa)).FirstOrDefault();

        //            var responseRec = await _client.PutAsync($"{_apiConsultaUrl}/api/Consulta/{consulta.Id}/recusar", content);

        //            Assert.Equal(HttpStatusCode.OK, responseRec.StatusCode);

        //        }
           
        //    }


        
        //    //var consulta = EnsureAnyAgendaMedicosExists();
           
        //}
        private async Task<ConsultaResponse> EnsureAnyAgendaPacienteExists()
        {
            // Autenticate
            await Login("12345678901", "123456");

            //Act
            var response = await _client.GetAsync($"{_apiConsultaUrl}/api/Consulta/paciente");
            //Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();

            var lista = JsonSerializer.Deserialize<List<ConsultaResponse>>(content, jsonOptions)!;


            if (lista is not null && lista.Any())
                return lista.First();

            var request = new AgendarConsultaRequest
                     ("12345678900",
                        "Paciente Teste",
                          "CRM123456",
                           DateTime.UtcNow.AddDays(1),
                          "Pendente",
                          "Checkup"
                      );

            var insertResponse = await _client.PostAsJsonAsync($"{_apiConsultaUrl}/api/Consulta/agendar", request);
            insertResponse.EnsureSuccessStatusCode();
            System.Threading.Thread.Sleep(50000);
            var responseretry = await _client.GetAsync($"{_apiConsultaUrl}/api/Consulta/paciente");
            responseretry.EnsureSuccessStatusCode();
            responseretry.StatusCode.Should().Be(HttpStatusCode.OK);
            var contentretry = await response.Content.ReadAsStringAsync();
            var atualizada = JsonSerializer.Deserialize<List<ConsultaResponse>>(content, jsonOptions)!;




            return atualizada.FirstOrDefault();
        }

        private async Task<ConsultaResponse> EnsureAnyAgendaMedicosExists()
        {
            // Autenticate
            await Login();

            //Act
            var response = await _client.GetAsync($"{_apiConsultaUrl}/api/Consulta/medico");
            //Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();

            var lista = JsonSerializer.Deserialize<List<ConsultaResponse>>(content, jsonOptions)!;


            if (lista is not null && lista.Any())
                return lista.FirstOrDefault();


            await Login("12345678901", "123456");
            var request = new AgendarConsultaRequest
                     ("12345678900",
                        "Paciente Teste",                           
                          "CRM123456",
                           DateTime.UtcNow.AddDays(1),
                          "Pendente",
                          "Checkup"
                      );

            var insertResponse = await _client.PostAsJsonAsync($"{_apiConsultaUrl}/api/Consulta/agendar", request);
            insertResponse.EnsureSuccessStatusCode();
            System.Threading.Thread.Sleep(50000);

            await Login();

            var responseretry = await _client.GetAsync($"{_apiConsultaUrl}/api/Consulta/medico");
            responseretry.EnsureSuccessStatusCode();
            responseretry.StatusCode.Should().Be(HttpStatusCode.OK);
            var contentretry = await response.Content.ReadAsStringAsync();
            var atualizada = JsonSerializer.Deserialize<List<ConsultaResponse>>(content, jsonOptions)!;




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