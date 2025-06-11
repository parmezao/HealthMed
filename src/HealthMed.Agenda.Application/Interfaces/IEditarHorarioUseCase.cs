using HealthMed.Agenda.Application.ViewModels;

namespace HealthMed.Agenda.Application.Interfaces
{
    public interface IEditarHorarioUseCase
    {
        Task<PublishResponse> ExecuteAsync(EditarHorarioRequest request);
    }
}
