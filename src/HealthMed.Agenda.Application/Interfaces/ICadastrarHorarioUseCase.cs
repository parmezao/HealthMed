using HealthMed.Agenda.Application.ViewModels;

namespace HealthMed.Agenda.Application.Interfaces
{
    public interface ICadastrarHorarioUseCase
    {
        Task<PublishResponse> ExecuteAsync(CadastrarHorarioRequest request);
    }
}
