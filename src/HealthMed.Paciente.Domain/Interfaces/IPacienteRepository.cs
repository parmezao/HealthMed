using HealthMed.Patient.Domain.Core;
using HealthMed.Patient.Domain.Entities;

namespace HealthMed.Patient.Domain.Interfaces
{
    public  interface IPacienteRepository : IRepository
    {
        Task<Paciente?> ObterPorCpfAsync(string cpf);
        Task<Paciente?> ObterPorIdAsync(int id);
        Task<List<Paciente>> ObterTodosAsync();
        Task AdicionarAsync(Paciente paciente);
        Task AtualizarAsync(Paciente paciente);
        Task RemoverAsync(Paciente paciente);
    }
}
