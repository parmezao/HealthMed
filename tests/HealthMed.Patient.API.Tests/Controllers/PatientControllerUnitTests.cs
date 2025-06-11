using HealthMed.Patient.API.Controllers;
using HealthMed.Patient.Application.Interfaces;
using HealthMed.Patient.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HealthMed.Patient.API.Tests.Controllers
{
    public class PatientControllerUnitTests
    {
        private readonly PacienteController _controller;
        public PatientControllerUnitTests()
        {
            _controller = new PacienteController();        
        }

        [Theory]
        [InlineData("nome", "1234567890", "email@email.com")]
        public async Task Cadastrar_ReturnsOkObjectResult_WhenCalled(string nome, string cpf, string email)
        {
            // Arrange
            var mockUseCase = new Mock<IInsertPacienteUseCase>();    
            var insertRequest = new InsertPacienteRequest(nome, cpf, email);
            mockUseCase.Setup(service => service.Execute(insertRequest)).ReturnsAsync(new PublishResponse());
            
            // Act
            var requestResult = await _controller.CadastrarAsync(mockUseCase.Object, insertRequest);

            // Assert
            Assert.IsType<OkObjectResult>(requestResult);
            Assert.IsType<PublishResponse>(((OkObjectResult)requestResult).Value);
        }

        [Theory]
        [InlineData(1, "nome", "1234567890", "email@email.com")]
        public async Task Atualizar_ReturnsOkObjectResult_WhenCalled(int id, string nome, string cpf, string email)
        {
            // Arrange
            var mockUseCase = new Mock<IUpdatePacienteUseCase>();
            var updateRequest = new UpdatePacienteRequest(id, nome, cpf, email);

            mockUseCase.Setup(service => service.Execute(updateRequest)).ReturnsAsync(new PublishResponse());

            // Act
            var requestResult = await _controller.AtualizarAsync(mockUseCase.Object, updateRequest);

            // Assert
            Assert.IsType<OkObjectResult>(requestResult);
            Assert.IsType<PublishResponse>(((OkObjectResult)requestResult).Value);
        }

        [Theory]
        [InlineData(1, "nome", "1234567890", "email@email.com")]
        public async Task ObterPorCpfAsync_ReturnsOkObjectResult_WhenPacienteExists(int id, string nome, string cpf, string email)
        {
            // Arrange
            var mockUseCase = new Mock<IGetPacienteUseCase>();
            var getResponse = new PacienteResponse(id, nome, email, cpf);

            mockUseCase.Setup(service => service.ObterPorCpfAsync(cpf)).ReturnsAsync(getResponse);

            // Act
            var requestResult = await _controller.ObterPorCpfAsync(mockUseCase.Object, cpf);

            // Assert
            Assert.IsType<OkObjectResult>(requestResult);
            Assert.IsType<PacienteResponse>(((OkObjectResult)requestResult).Value);
        }

        [Theory]
        [InlineData(1, "nome", "1234567890", "email@email.com")]
        public async Task ObterPorIdAsync_ReturnsOkObjectResult_WhenPacienteExists(int id, string nome, string cpf, string email)
        {
            // Arrange
            var mockUseCase = new Mock<IGetPacienteUseCase>();
            var getResponse = new PacienteResponse(id, nome, email, cpf);

            mockUseCase.Setup(service => service.ObterPorIdAsync(id)).ReturnsAsync(getResponse);

            // Act
            var requestResult = await _controller.ObterPorIdAsync(mockUseCase.Object, id);

            // Assert
            Assert.IsType<OkObjectResult>(requestResult);
            Assert.IsType<PacienteResponse>(((OkObjectResult)requestResult).Value);
        }

        [Fact]
        public async Task ObterTodosAsync_ReturnsOkObjectResult_WhenPacienteExists()
        {
            // Arrange
            var mockUseCase = new Mock<IGetPacienteUseCase>();
            var getResponse = new List<PacienteResponse>()
            {
                new PacienteResponse(1, "nome1", "1234567891", "email1@email.com"),
                new PacienteResponse(2, "nome2", "1234567892", "email2@email.com"),
            };

            mockUseCase.Setup(service => service.ObterTodosAsync()).ReturnsAsync(new List<PacienteResponse>());

            // Act
            var requestResult = await _controller.ObterTodosAsync(mockUseCase.Object);

            // Assert
            Assert.IsType<OkObjectResult>(requestResult);
            Assert.IsType<List<PacienteResponse>>(((OkObjectResult)requestResult).Value);
        }
    }
}