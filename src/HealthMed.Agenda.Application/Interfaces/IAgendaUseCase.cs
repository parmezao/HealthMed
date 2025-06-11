using HealthMed.Agenda.Application.ViewModels;

namespace HealthMed.Agenda.Application.Interfaces
{
    public  interface IAgendaUseCase
    {
        Task CadastrarHorarioAsync(CadastrarHorarioRequest request);
        Task<List<HorarioDisponivelResponse>> ObterPorMedicoAsync(int medicoId);
        Task<HorarioDisponivelResponse?> ObterPorIdAsync(int id);
        Task MarcarComoOcupadoAsync(int horarioId);
    }
}
