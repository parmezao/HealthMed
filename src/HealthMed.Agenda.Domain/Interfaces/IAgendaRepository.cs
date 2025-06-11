using HealthMed.Agenda.Domain.Core;
using HealthMed.Agenda.Domain.Entities;

namespace HealthMed.Agenda.Domain.Interfaces
{
    public interface IAgendaRepository : IRepository
    {
        Task<List<HorarioDisponivel>> ObterPorMedicoAsync(int medicoId);
        Task<HorarioDisponivel?> ObterPorIdAsync(int id);
        Task AdicionarAsync(HorarioDisponivel horario);
        Task AtualizarAsync(HorarioDisponivel horario);
        Task RemoverAsync(HorarioDisponivel horario);
        Task MarcarComoOcupadoAsync(int id);
    }
}
