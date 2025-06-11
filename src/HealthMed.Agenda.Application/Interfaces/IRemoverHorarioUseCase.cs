using HealthMed.Agenda.Application.ViewModels;

namespace HealthMed.Agenda.Application.Interfaces
{
    public interface IRemoverHorarioUseCase
    {
        Task<PublishResponse> ExecuteAsync(RemoverHorarioRequest request);
    }
}
