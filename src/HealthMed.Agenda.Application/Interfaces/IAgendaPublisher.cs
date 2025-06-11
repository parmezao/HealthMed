using HealthMed.Agenda.Application.Events;

namespace HealthMed.Agenda.Application.Interfaces
{
    public interface IAgendaPublisher
    {
        Task PublishCadastrarHorarioAsync(CadastrarHorarioEvent message);
        Task PublishEditarHorarioAsync(EditarHorarioEvent message);
        Task PublishRemoverHorarioAsync(RemoverHorarioEvent message);
    }
}
