
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HealthMed.Auth.Application.Interfaces;
using HealthMed.Auth.API.Controllers;
using HealthMed.Auth.Application.ViewModels;

namespace HealthMed.Auth.API.Tests.Controllers
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task Login_ReturnsOk_WhenCredentialsAreValid()
        {
            var useCase = new Mock<ILoginUseCase>();
            var logger = new Mock<ILogger<AuthController>>();
            useCase.Setup(u => u.ExecuteAsync(It.IsAny<LoginRequest>())).ReturnsAsync(new LoginResponse() { Token = "eyJhbGciOiJIUzI1N" , Nome = "Dr. João Silva", Role="medico"});

            var controller = new AuthController(useCase.Object, logger.Object);

            var result = await controller.Login(new LoginRequest { Login = "user", Senha = "123" });

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenUserNotFound()
        {
            var useCase = new Mock<ILoginUseCase>();
            var logger = new Mock<ILogger<AuthController>>();
            useCase.Setup(u => u.ExecuteAsync(It.IsAny<LoginRequest>())).ReturnsAsync((LoginResponse)null);

            var controller = new AuthController(useCase.Object, logger.Object);

            var result = await controller.Login(new LoginRequest { Login = "invalid", Senha = "wrong" });

            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}
