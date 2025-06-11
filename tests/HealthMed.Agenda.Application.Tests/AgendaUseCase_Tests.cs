
using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthMed.Agenda.Domain.Interfaces;
using HealthMed.Agenda.Domain.Entities;
using HealthMed.Agenda.Application.UseCases;


namespace HealthMed.Agenda.Application.Tests.UseCases
{
    public class AgendaUseCase_Tests
    {
        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarHorario()
        {
            var repo = new Mock<IAgendaRepository>();
            repo.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(new HorarioDisponivel
            {
                Id = 1, MedicoId = 2, DataHora = DateTime.Now, Ocupado = false
            });

            var useCase = new AgendaUseCase(repo.Object);
            var result = await useCase.ObterPorIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveLancarExcecao_SeHorarioNaoEncontrado()
        {
            var repo = new Mock<IAgendaRepository>();
            repo.Setup(r => r.ObterPorIdAsync(999)).ReturnsAsync((HorarioDisponivel)null);

            var useCase = new AgendaUseCase(repo.Object);

            await Assert.ThrowsAsync<ApplicationException>(() => useCase.ObterPorIdAsync(999));
        }

        [Fact]
        public async Task MarcarComoOcupadoAsync_DeveChamarRepositorio()
        {
            var repo = new Mock<IAgendaRepository>();
            var useCase = new AgendaUseCase(repo.Object);

            await useCase.MarcarComoOcupadoAsync(10);

            repo.Verify(r => r.MarcarComoOcupadoAsync(10), Times.Once);
        }
    }
}
