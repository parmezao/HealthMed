using HealthMed.Agenda.Application.Events;
using HealthMed.Agenda.Application.Interfaces;
using MassTransit;

namespace HealthMed.Agenda.Application.Publisher
{
    public class AgendaPublisher : IAgendaPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;
        public AgendaPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public async Task PublishRemoverHorarioAsync(RemoverHorarioEvent message)
        {
            await _publishEndpoint.Publish(message);
        }

        public async Task PublishEditarHorarioAsync(EditarHorarioEvent message)
        {
            await _publishEndpoint.Publish(message);
        }

        public async Task PublishCadastrarHorarioAsync(CadastrarHorarioEvent message)
        {
            await _publishEndpoint.Publish(message);
        }
    }
}
