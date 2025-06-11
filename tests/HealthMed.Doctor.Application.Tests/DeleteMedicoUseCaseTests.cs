
using Xunit;
using Moq;
using System.Threading.Tasks;
using HealthMed.Doctor.Application.UseCases;
using HealthMed.Doctor.Application.Events;
using HealthMed.Doctor.Application.Interfaces;
using HealthMed.Doctor.Domain.Entities;
using HealthMed.Doctor.Domain.Interfaces;

namespace HealthMed.Doctor.Application.Tests.UseCases
{
    public class DeleteMedicoUseCaseTests
    {
        [Fact]
        public async Task Delete_Should_Publish_DeleteMedicoEvent()
        {
            var publisherMock = new Mock<IMedicoPublisher>();
            var repoMock = new Mock<IMedicoRepository>();

            repoMock.Setup(r => r.ObterPorIdAsync(5)).ReturnsAsync(new Medico("Dr. Delete", "Clínico", "CRM005", new()));

            var useCase = new DeleteMedicoUseCase(publisherMock.Object, repoMock.Object);

            var response = await useCase.Delete(5);

            Assert.NotNull(response);
            Assert.Equal("Exclusão em processamento.", response.Message);

            publisherMock.Verify(p => p.PublishDeleteMedicotAsync(It.Is<DeleteMedicoEvent>(e => e.Id == 5)), Times.Once);
        }
    }
}
