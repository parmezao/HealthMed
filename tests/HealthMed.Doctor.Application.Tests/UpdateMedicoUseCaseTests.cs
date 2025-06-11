
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthMed.Doctor.Application.UseCases;
using HealthMed.Doctor.Application.ViewModels;
using HealthMed.Doctor.Application.Events;
using HealthMed.Doctor.Application.Interfaces;
using HealthMed.Doctor.Domain.Entities;
using HealthMed.Doctor.Domain.Interfaces;

namespace HealthMed.Doctor.Application.Tests.UseCases
{
    public class UpdateMedicoUseCaseTests
    {
        [Fact]
        public async Task Execute_Should_Publish_UpdateMedicoEvent()
        {
            var publisherMock = new Mock<IMedicoPublisher>();
            var repoMock = new Mock<IMedicoRepository>();

            repoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(new Medico("Dr. Atual", "Ortopedia", "CRM456", new()));

            var useCase = new UpdateMedicoUseCase(publisherMock.Object, repoMock.Object);

            var request = new UpdateMedicoRequest
            (
                1,
                "Dr. Atualizado",
                 "Neurologia",
                 "CRM999",
                 new List<HorarioDto>
                {
                    new HorarioDto { DataHora = System.DateTime.Now, Ocupado = true }
                }
            );

            var response = await useCase.Execute(request);

            Assert.NotNull(response);
            Assert.Equal("Atualização em processamento.", response.Message);

            publisherMock.Verify(p => p.PublishUpdateMedicoAsync(It.Is<UpdateMedicoEvent>(e =>
                e.Id == request.Id &&
                e.Nome == request.Nome &&
                e.Especialidade == request.Especialidade &&
                e.CRM == request.CRM
            )), Times.Once);
        }
    }
}
