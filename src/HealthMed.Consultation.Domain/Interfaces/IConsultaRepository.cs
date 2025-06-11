using HealthMed.Consultation.Domain.Core;
using HealthMed.Consultation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Consultation.Domain.Interfaces
{
    public interface IConsultaRepository : IRepository
    {
        Task AgendarAsync(Consulta consulta);
        Task AtualizarStatusAsync(Consulta consulta);
        Task<List<Consulta>> ObterPorCrmAsync(string crm);
        Task<List<Consulta>> ObterPorCpfAsync(string cpf);

        Task<Consulta> ObterPorIdAsync(int Id);
    }

}
