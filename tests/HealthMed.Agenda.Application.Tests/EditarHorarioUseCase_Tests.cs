
using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using HealthMed.Agenda.Application.UseCases;
using HealthMed.Agenda.Application.Interfaces;
using HealthMed.Agenda.Application.ViewModels;
using HealthMed.Agenda.Application.Events;

namespace HealthMed.Agenda.Application.Tests.UseCases
{
    public class EditarHorarioUseCase_Tests
    {
        [Fact]
        public async Task ExecuteAsync_DevePublicarEvento()
        {
            var publisher = new Mock<IAgendaPublisher>();
            var useCase = new EditarHorarioUseCase(publisher.Object);

            var request = new EditarHorarioRequest ( 1,  DateTime.Now.AddHours(2));

            var result = await useCase.ExecuteAsync(request);

            Assert.NotNull(result);
            Assert.Equal("Alteração em processamento.", result.Message);

            publisher.Verify(p => p.PublishEditarHorarioAsync(It.IsAny<EditarHorarioEvent>()), Times.Once);
        }
    }
}
