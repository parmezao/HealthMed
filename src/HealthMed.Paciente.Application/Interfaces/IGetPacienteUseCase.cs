using HealthMed.Patient.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Patient.Application.Interfaces
{
  public  interface IGetPacienteUseCase
    {
        Task<PacienteResponse?> ObterPorCpfAsync(string cpf);
        Task<PacienteResponse?> ObterPorIdAsync(int id);
        Task<List<PacienteResponse>> ObterTodosAsync();
    }
}
