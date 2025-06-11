
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthMed.Doctor.Application.UseCases;
using HealthMed.Doctor.Application.ViewModels;
using HealthMed.Doctor.Application.Events;
using HealthMed.Doctor.Application.Interfaces;

namespace HealthMed.Doctor.Application.Tests.UseCases
{
    public class InsertMedicoUseCaseTests
    {
        [Fact]
        public async Task Execute_Should_Publish_InsertMedicoEvent()
        {
            var publisherMock = new Mock<IMedicoPublisher>();
            var useCase = new InsertMedicoUseCase(publisherMock.Object);

            var request = new InsertMedicoRequest
           (  "Dr. Novo",
                "Pediatria",
                "CRM123",
                  new List<HorarioDto>
                {
                    new HorarioDto { DataHora = System.DateTime.Now, Ocupado = false }
                }
            );

            var response = await useCase.Execute(request);

            Assert.NotNull(response);
            Assert.Equal("Cadastro em processamento.", response.Message);

            publisherMock.Verify(p => p.PublishInsertMedicoAsync(It.Is<InsertMedicoEvent>(e =>
                e.Nome == request.Nome &&
                e.Especialidade == request.Especialidade &&
                e.CRM == request.CRM &&
                e.Horarios.Count == 1
            )), Times.Once);
        }
    }
}
