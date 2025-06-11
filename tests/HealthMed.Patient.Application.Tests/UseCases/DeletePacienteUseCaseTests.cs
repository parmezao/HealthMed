
using Xunit;
using Moq;
using System.Threading.Tasks;
using HealthMed.Patient.Application.UseCases;
using HealthMed.Patient.Application.Interfaces;
using HealthMed.Patient.Application.Events;
using HealthMed.Patient.Domain.Entities;
using HealthMed.Patient.Domain.Interfaces;

namespace HealthMed.Patient.Application.Tests.UseCases
{
    public class DeletePacienteUseCaseTests
    {
        [Fact]
        public async Task Delete_DevePublicarEvento_QuandoEncontrado()
        {
            var publisher = new Mock<IPacientePublisher>();
            var repo = new Mock<IPacienteRepository>();
            repo.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(new Paciente("Del", "33333333322", "email@email.com") { Id = 1 });

            var useCase = new DeletePacienteUseCase(publisher.Object, repo.Object);

            var result = await useCase.Delete(1);

            Assert.Equal("Exclusão em processamento.", result.Message);
            publisher.Verify(p => p.PublishDeletePacienteAsync(It.Is<DeletePacienteEvent>(e => e.Id == 1)), Times.Once);
        }

        [Fact]
        public async Task Delete_DeveLancarErro_SeNaoEncontrado()
        {
            var publisher = new Mock<IPacientePublisher>();
            var repo = new Mock<IPacienteRepository>();
            repo.Setup(r => r.ObterPorIdAsync(99)).ReturnsAsync((Paciente)null);

            var useCase = new DeletePacienteUseCase(publisher.Object, repo.Object);

            await Assert.ThrowsAsync<ApplicationException>(() => useCase.Delete(99));
        }
    }
}
