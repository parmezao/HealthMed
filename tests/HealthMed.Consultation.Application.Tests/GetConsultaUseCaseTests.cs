using HealthMed.Consultation.Application.UseCases;
using HealthMed.Consultation.Domain.Entities;
using HealthMed.Consultation.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Consultation.Application.Tests
{
    public class GetConsultaUseCaseTests
    {
        [Fact]
        public async Task ListarPorCpfAsync_DeveRetornarConsultas()
        {
            var mockRepo = new Mock<IConsultaRepository>();
            mockRepo.Setup(x => x.ObterPorCpfAsync("123"))
                .ReturnsAsync(new List<Consulta> { new  Consulta(   1,  "123",  "CRM123",   "Paciente",   DateTime.Now ) });

            var useCase = new GetConsultaUseCase(mockRepo.Object);

            var result = await useCase.ListarPorCpfAsync("123");

            Assert.Single(result);
            Assert.Equal("123", result[0].CpfPaciente);
        }
    }
}
