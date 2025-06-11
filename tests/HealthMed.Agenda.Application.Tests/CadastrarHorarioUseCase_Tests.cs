
using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using HealthMed.Agenda.Application.UseCases;
using HealthMed.Agenda.Application.ViewModels;
using HealthMed.Agenda.Application.Interfaces;
using HealthMed.Agenda.Application.Events;

namespace HealthMed.Agenda.Application.Tests.UseCases
{
    public class CadastrarHorarioUseCase_Tests
    {
        [Fact]
        public async Task ExecuteAsync_DevePublicarEvento()
        {
            var publisher = new Mock<IAgendaPublisher>();
            var useCase = new CadastrarHorarioUseCase(publisher.Object);

            var request = new CadastrarHorarioRequest (3, DateTime.Now.AddHours(1) ,100 );

            var result = await useCase.ExecuteAsync(request);

            Assert.NotNull(result);
            Assert.Equal("Cadastro em processamento.", result.Message);

            publisher.Verify(p => p.PublishCadastrarHorarioAsync(It.IsAny<CadastrarHorarioEvent>()), Times.Once);
        }
    }
}
