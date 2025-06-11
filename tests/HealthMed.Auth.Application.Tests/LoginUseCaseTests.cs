
using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using HealthMed.Auth.Application.UseCases;
using HealthMed.Auth.Application.ViewModels;
using HealthMed.Auth.Domain.Entities;
using HealthMed.Auth.Domain.Interfaces;

namespace HealthMed.Auth.Application.Tests.UseCases
{
    public class LoginUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_ValidCredentials_ReturnsToken()
        {
            var config = new Mock<IConfiguration>();
            config.Setup(c => c["SecretJWT"]).Returns("MySuperSecretKey12345678901234567890");

            var medico = new Medico
            {
                Nome = "Dr. Teste",
                Email = "medico@email.com",
                SenhaHash = "senha123"
            };

            var repo = new Mock<IUsuarioRepository>();
            repo.Setup(r => r.ObterPorLoginAsync("medico@email.com")).ReturnsAsync(medico);
            repo.Setup(r => r.VerificarSenhaAsync(medico, "123456")).ReturnsAsync(true);

            var useCase = new LoginUseCase(config.Object, repo.Object);
            var request = new LoginRequest { Login = "medico@email.com", Senha = "123456" };

            var result = await useCase.ExecuteAsync(request);

            Assert.NotNull(result);
            Assert.Equal("medico", result.Role);
        }
    }

   
}
 
 
