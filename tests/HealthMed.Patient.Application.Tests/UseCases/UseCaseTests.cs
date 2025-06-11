using FluentAssertions;
using HealthMed.Patient.Application.Interfaces;
using HealthMed.Patient.Application.UseCases;
using HealthMed.Patient.Application.ViewModels;
using HealthMed.Patient.Domain.Entities;
using HealthMed.Patient.Domain.Interfaces;
using Moq;
namespace HealthMed.Patient.Application.Tests.UseCases
{
    public class UseCaseTests
    {
        private readonly Mock<IPacientePublisher> _publisherMock;
        private readonly Mock<IPacienteRepository> _repositoryMock;
        private readonly Paciente _pacienteMock;
        public UseCaseTests()
        {
            _publisherMock = new Mock<IPacientePublisher>();
            _repositoryMock = new Mock<IPacienteRepository>();
            _pacienteMock = new Paciente
            {
                Id = 1,
                Nome = "nome",
                Cpf = "1234567890",
                Email = "email@email.com"
            };
            _repositoryMock.Setup(repo => repo.ObterPorIdAsync(_pacienteMock.Id)).ReturnsAsync(_pacienteMock);
            _repositoryMock.Setup(repo => repo.ObterPorCpfAsync(_pacienteMock.Cpf)).ReturnsAsync(_pacienteMock);
        }

        [Theory]
        [InlineData("nome","1234567890","email@email.com", "Cadastro em processamento.")]
        public async Task InsertPacienteUseCaseExecute_ReturnsPublishResponseWithRequestData_WhenCalled(string nome, string cpf, string email, string resultMessage)
        {
            // Arrange
            var useCase = new InsertPacienteUseCase(_publisherMock.Object);
            var request = new InsertPacienteRequest(nome, cpf, email);
            var useCaseResultExpected = new PublishResponse
            {
                Message = resultMessage,
                Data = new
                {
                    request.Nome,
                    request.Email,
                    request.Cpf
                }
            };

            // Act
            var useCaseResult = await useCase.Execute(request);

            // Assert
            useCaseResult.Should().BeEquivalentTo(useCaseResultExpected);
        }

        [Theory]
        [InlineData(1, "nome", "1234567890", "email@email.com", "Atualização em processamento.")]
        public async Task UpdatePacienteUseCaseExecute_ReturnsPublishResponseWithRequestData_WhenCalled(int id, string nome, string cpf, string email, string resultMessage)
        {
            // Arrange
            var useCase = new UpdatePacienteUseCase(_publisherMock.Object, _repositoryMock.Object);
            var request = new UpdatePacienteRequest(id, nome, cpf, email);
            var useCaseResultExpected = new PublishResponse
            {
                Message = resultMessage,
                Data = new
                {
                    request.Id,
                    request.Nome,
                    request.Email,
                    request.Cpf
                }
            };

            // Act
            var useCaseResult = await useCase.Execute(request);

            // Assert
            useCaseResult.Should().BeEquivalentTo(useCaseResultExpected);
        }

        [Theory]
        [InlineData(2, "Paciente não encontrado!")]
        public async Task GetPacienteUseCase_ObterPorId_ReturnsApplicationException_WhenPacienteNotExists(
            int id,
            string errorMessage)
        {
            // Arrange
            var useCase = new GetPacienteUseCase(_repositoryMock.Object);

            // Act
            var useCaseResult = await Assert.ThrowsAsync<ApplicationException>(() => useCase.ObterPorIdAsync(id));

            // Assert
            Assert.Equal(errorMessage, useCaseResult.Message);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetPacienteUseCase_ObterPorId_ReturnsSuccessWithPacienteData_WhenPacienteExists(int id)
        {
            // Arrange
            var useCase = new GetPacienteUseCase(_repositoryMock.Object);
            var useCaseResultExpected = new PacienteResponse(_pacienteMock.Id, _pacienteMock.Nome, _pacienteMock.Cpf, _pacienteMock.Email);

            // Act
            var useCaseResult = await useCase.ObterPorIdAsync(id);

            // Assert
            useCaseResult.Should().BeEquivalentTo(useCaseResultExpected);
        }

        [Theory]
        [InlineData("0000000000", "Paciente não encontrado!")]
        public async Task GetPacienteUseCase_ObterPorCpf_ReturnsApplicationException_WhenPacienteNotExists(
            string cpf,
            string errorMessage)
        {
            // Arrange
            var useCase = new GetPacienteUseCase(_repositoryMock.Object);

            // Act
            var useCaseResult = await Assert.ThrowsAsync<ApplicationException>(() => useCase.ObterPorCpfAsync(cpf));

            // Assert
            Assert.Equal(errorMessage, useCaseResult.Message);
        }

        [Theory]
        [InlineData("1234567890")]
        public async Task GetPacienteUseCase_ObterPorCpf_ReturnsSuccessWithPacienteData_WhenPacienteExists(string cpf)
        {
            // Arrange
            var useCase = new GetPacienteUseCase(_repositoryMock.Object);
            var useCaseResultExpected = new PacienteResponse(_pacienteMock.Id, _pacienteMock.Nome, _pacienteMock.Cpf, _pacienteMock.Email);

            // Act
            var useCaseResult = await useCase.ObterPorCpfAsync(cpf);
            // Assert
            useCaseResult.Should().BeEquivalentTo(useCaseResultExpected);
        }

        [Fact]
        public async Task GetPacienteUseCase_ObterTodos_ReturnsApplicationException_WhenThereAreNoPaciente()
        {
            // Arrange
            var useCase = new GetPacienteUseCase(_repositoryMock.Object);
            _repositoryMock.Setup(repo => repo.ObterTodosAsync()).ReturnsAsync(new List<Paciente>());

            // Act
            var useCaseResult = await useCase.ObterTodosAsync();

            // Assert
            Assert.Empty(useCaseResult);
        }

        [Fact]
        public async Task GetPacienteUseCase_ObterTodos_ReturnsSuccessWithPacienteData_WhenExistsPaciente()
        {
            // Arrange
            var useCase = new GetPacienteUseCase(_repositoryMock.Object);
            var useCaseResultExpected = new List<PacienteResponse>
            {
                new(_pacienteMock.Id, _pacienteMock.Nome, _pacienteMock.Cpf, _pacienteMock.Email)
            };
            
            _repositoryMock.Setup(repo => repo.ObterTodosAsync()).ReturnsAsync(new List<Paciente>()
            {
                new(){ Id = _pacienteMock.Id, Nome = _pacienteMock.Nome, Cpf = _pacienteMock.Cpf, Email = _pacienteMock.Email }
            });

            // Act
            var useCaseResult = await useCase.ObterTodosAsync();

            // Assert
            useCaseResult.Should().BeEquivalentTo(useCaseResultExpected);
        }
    }
}