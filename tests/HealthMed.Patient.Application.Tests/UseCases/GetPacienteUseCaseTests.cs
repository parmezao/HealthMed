
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthMed.Patient.Application.UseCases;
using HealthMed.Patient.Application.ViewModels;
using HealthMed.Patient.Domain.Entities;
using HealthMed.Patient.Domain.Interfaces;

namespace HealthMed.Patient.Application.Tests.UseCases
{
    public class GetPacienteUseCaseTests
    {
        [Fact]
        public async Task ObterPorCpfAsync_DeveRetornarPaciente()
        {
            var repo = new Mock<IPacienteRepository>();
            repo.Setup(r => r.ObterPorCpfAsync("12345678911")).ReturnsAsync(new Paciente("Nome", "12345678911", "email@email.com") { Id = 1 });

            var useCase = new GetPacienteUseCase(repo.Object);
            var result = await useCase.ObterPorCpfAsync("12345678911");

            Assert.NotNull(result);
            Assert.Equal("12345678911", result.Cpf);
        }

        [Fact]
        public async Task ObterPorCpfAsync_DeveLancarErro_SeNaoEncontrado()
        {
            var repo = new Mock<IPacienteRepository>();
            repo.Setup(r => r.ObterPorCpfAsync("12345678911")).ReturnsAsync((Paciente)null);
            var useCase = new GetPacienteUseCase(repo.Object);

            await Assert.ThrowsAsync<ApplicationException>(() => useCase.ObterPorCpfAsync("000"));
        }

        [Fact]
        public async Task ObterTodosAsync_DeveRetornarLista()
        {
            var repo = new Mock<IPacienteRepository>();
            repo.Setup(r => r.ObterTodosAsync()).ReturnsAsync(new List<Paciente>
            {
                new Paciente("Addd", "12345678911", "email@email.com")
            });

            var useCase = new GetPacienteUseCase(repo.Object);
            var result = await useCase.ObterTodosAsync();

            Assert.Single(result);
        }
    }
}
