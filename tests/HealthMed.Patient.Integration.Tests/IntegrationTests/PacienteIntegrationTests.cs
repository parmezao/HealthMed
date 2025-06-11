using Bogus;
using FluentAssertions;
using HealthMed.Auth.Application.ViewModels;
using HealthMed.Patient.Application.ViewModels;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HealthMed.Patient.Integration.Tests.IntegrationTests
{
    public class PacienteIntegrationTests
    {
        private readonly Faker _faker;
        private readonly HttpClient _client;
        private readonly string _authUrl = "http://localhost:8081";
        private readonly string _apiPacienteUrl = "http://localhost:8084";

        public PacienteIntegrationTests()
        {
            _client = new HttpClient();
            _faker = new Faker(locale: "pt_BR");
        }

        [Fact]
        public async Task IntegrationPaciente_ShouldInsertAndGetInsertedFromDatabase()
        {
            // Insert Paciente In Queue
            var pacienteRequest = new InsertPacienteRequest(_faker.Name.FullName(), _faker.Random.String2(11, "0123456789"), _faker.Internet.Email());

            var cadastrarResponse = await _client.PostAsJsonAsync($"{_apiPacienteUrl}/api/paciente/cadastrar", pacienteRequest);
            cadastrarResponse.EnsureSuccessStatusCode();

            var publishedResponse = await cadastrarResponse.Content.ReadFromJsonAsync<PublishResponse>();
            publishedResponse.Should().NotBeNull();

            publishedResponse.Message.Should().Be("Cadastro em processamento.");
            publishedResponse.Data.Should().NotBeNull();

            var publishedResponseData = JsonConvert.DeserializeObject<PacienteResponse>(publishedResponse.Data.ToString());
            publishedResponseData!.Should().BeEquivalentTo(pacienteRequest);

            // Autenticate
            await this.Login();

            var obterPorCpfResponse = await _client.GetAsync($"{_apiPacienteUrl}/api/paciente/obterPorCpf?cpf={pacienteRequest.Cpf}");
            var pacienteFromDatabase = await obterPorCpfResponse.Content.ReadFromJsonAsync<PacienteResponse>();

            // Validate Published/Consumed and Inserted Paciente In Database
            pacienteFromDatabase.Should().NotBeNull();
            pacienteFromDatabase.Should().BeEquivalentTo(pacienteRequest);
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
