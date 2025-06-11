
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthMed.Doctor.Application.UseCases;
using HealthMed.Doctor.Domain.Entities;
using HealthMed.Doctor.Domain.Interfaces;

namespace HealthMed.Doctor.Application.Tests.UseCases
{
    public class GetMedicosUseCaseTests
    {
        [Fact]
        public async Task GetAll_Should_Return_List()
        {
            var repoMock = new Mock<IMedicoRepository>();
            repoMock.Setup(r => r.GetAll("")).ReturnsAsync(new List<Medico>
            {
                new Medico("Dr. A", "Cardiologia", "CRM001", new())
            });

            var useCase = new GetMedicosUseCase(repoMock.Object);

            var result = await useCase.GetAll("");

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("CRM001", result[0].CRM);
        }
    }
}
