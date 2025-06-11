using HealthMed.Patient.Application.Interfaces;
using HealthMed.Patient.Application.ViewModels;
using HealthMed.Patient.Domain.Interfaces;

namespace HealthMed.Patient.Application.UseCases
{
    public class GetPacienteUseCase(IPacienteRepository  pacienteRepository) : IGetPacienteUseCase
    {
        public async Task<PacienteResponse?> ObterPorCpfAsync(string cpf)
        {
            var paciente = await pacienteRepository.ObterPorCpfAsync(cpf);
            if (paciente is null)
                throw new ApplicationException("Paciente não encontrado!");
            return new PacienteResponse(paciente.Id, paciente.Nome, paciente.Cpf, paciente.Email);
        }

        public async Task<PacienteResponse?> ObterPorIdAsync(int id)
        {
            var paciente = await pacienteRepository.ObterPorIdAsync(id);
            if (paciente is null)
                throw new ApplicationException("Paciente não encontrado!");
            return new PacienteResponse(paciente.Id, paciente.Nome, paciente.Cpf, paciente.Email);
        }

        public async Task<List<PacienteResponse>> ObterTodosAsync()
        {
            var result = await pacienteRepository.ObterTodosAsync();
            var mapped = result.Select(x => new PacienteResponse(x.Id, x.Nome, x.Cpf, x.Email)).ToList();
            return mapped;
        }
    }
}
