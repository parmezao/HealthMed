using HealthMed.Consultation.Application.Interfaces;
using HealthMed.Consultation.Application.UseCases;
using HealthMed.Consultation.Application.ViewModels;
using HealthMed.Consultation.Domain.Entities;
using HealthMed.Consultation.Domain.Interfaces;
using Xunit;
using Moq;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthMed.Consultation.Application.Events;

namespace HealthMed.Consultation.Application.Tests
{
    public class AtualizarStatusUseCaseTests
    {
        [Fact]
        public async Task Execute_DevePublicarEventoEAtualizarStatus()
        {
            var mockPublisher = new Mock<IConsultaPublisher>();
            var mockRepo = new Mock<IConsultaRepository>();

            mockRepo.Setup(r => r.ObterPorIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(new Consulta(1, "", "", "", DateTime.Now));

            var useCase = new AtualizarStatusUseCase(mockPublisher.Object, mockRepo.Object);
            var request = new AtualizarStatusRequest { ConsultaId = 1, NovoStatus = "Aceita", Justificativa = "" };

            var result = await useCase.Execute(request);

            mockPublisher.Verify(x => x.PublishAtualizarStatusAsync(It.IsAny<AtualizarStatusEvent>()), Times.Once);
            Assert.Equal("Atualização em processamento.", result.Message);
        }
    }
}
