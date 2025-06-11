using HealthMed.Consultation.Application.Events;
using HealthMed.Consultation.Application.Interfaces;
using HealthMed.Consultation.Application.UseCases;
using HealthMed.Consultation.Application.ViewModels;
using Moq;
using Xunit;
namespace HealthMed.Consultation.Application.Tests
{
    public class AgendarConsultaUseCaseTests
    {
        [Fact]
        public async Task Execute_DeveChamarPublisherERetornarMensagem()
        {
            // Arrange
            var mockPublisher = new Mock<IConsultaPublisher>();
            var useCase = new AgendarConsultaUseCase(mockPublisher.Object);
            var request = new AgendarConsultaRequest
            (
                 "12345678900",
               "João",
                "CRM123",
                DateTime.UtcNow,
               "Agendada",
                 "checkup"
            );

            // Act
            var result = await useCase.Execute(request);

            // Assert
            mockPublisher.Verify(x => x.PublishAgendarConsultaAsync(It.IsAny<AgendarConsultaEvent>()), Times.Once);
            Assert.Equal("Cadastro em processamento.", result.Message);
        }

        [Fact]
        public async Task Execute_Should_Publish_Event_When_Valid()
        {
            var publisherMock = new Mock<IConsultaPublisher>();
            var useCase = new AgendarConsultaUseCase(publisherMock.Object);
            var request = new AgendarConsultaRequest
           (  "12345678900",
                  "Paciente",
                "CRM123",
                 DateTime.Now.AddDays(1),
                  "Agendada",
                  ""
            );

            var result = await useCase.Execute(request);

            Assert.NotNull(result);
            publisherMock.Verify(p => p.PublishAgendarConsultaAsync(It.IsAny<AgendarConsultaEvent>()), Times.Once);
        }


    }
}