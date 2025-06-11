using HealthMed.Doctor.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Application.Interfaces
{
   public interface IGetMedicosUseCase
    {
        Task<MedicoResponse> ObterPorCrmAsync(string crm);
        Task<MedicoResponse?> ObterPorIdAsync(int id);

        Task<List<MedicoResponse>> GetAll(string? especialidade);
    }
}
