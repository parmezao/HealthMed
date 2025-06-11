using HealthMed.Patient.Application.Events;
using HealthMed.Patient.Application.Interfaces;
using MassTransit;

namespace HealthMed.Patient.Application.Publisher
{
    public class PacientePublisher : IPacientePublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;
        public PacientePublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public async Task PublishDeletePacienteAsync(DeletePacienteEvent message)
        {
            await _publishEndpoint.Publish(message);
        }

        public async Task PublishInsertPacienteAsync(InsertPacienteEvent message)
        {
            await _publishEndpoint.Publish(message);
        }

        public async Task PublishUpdatePacienteAsync(UpdatePacienteEvent message)
        {
            await _publishEndpoint.Publish(message);
        }
    }
}
