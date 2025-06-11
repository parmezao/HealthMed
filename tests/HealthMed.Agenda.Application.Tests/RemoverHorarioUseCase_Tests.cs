
using Xunit;
using Moq;
using System.Threading.Tasks;
using HealthMed.Agenda.Application.UseCases;
using HealthMed.Agenda.Application.Interfaces;
using HealthMed.Agenda.Application.ViewModels;
using HealthMed.Agenda.Application.Events;

namespace HealthMed.Agenda.Application.Tests.UseCases
{
    public class RemoverHorarioUseCase_Tests
    {
        [Fact]
        public async Task ExecuteAsync_DevePublicarEvento()
        {
            var publisher = new Mock<IAgendaPublisher>();
            var useCase = new RemoverHorarioUseCase(publisher.Object);

            var request = new RemoverHorarioRequest(7);

            var result = await useCase.ExecuteAsync(request);

            Assert.NotNull(result);
            Assert.Equal("Exclusão em processamento.", result.Message);

            publisher.Verify(p => p.PublishRemoverHorarioAsync(It.IsAny<RemoverHorarioEvent>()), Times.Once);
        }
    }
}
