using HealthMed.Consultation.Application.Events;
using HealthMed.Consultation.Application.Interfaces;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Consultation.Application.UseCases
{
    public class ConsultaPublisher : IConsultaPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public ConsultaPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public async Task PublishAgendarConsultaAsync(AgendarConsultaEvent message)
        {
            await _publishEndpoint.Publish(message);
        }

        public async Task PublishAtualizarStatusAsync(AtualizarStatusEvent message)
        {
            await _publishEndpoint.Publish(message);
        }
    }
}
