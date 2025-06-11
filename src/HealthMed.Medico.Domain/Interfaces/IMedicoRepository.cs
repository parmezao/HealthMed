using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthMed.Doctor.Domain.Core;
using HealthMed.Doctor.Domain.Entities;
namespace HealthMed.Doctor.Domain.Interfaces
{
    public interface IMedicoRepository : IRepository
    {
        Task<Medico> ObterPorCrmAsync(string crm);

        void Save(Medico medico);
        void Update(Medico medico);
        void Delete(Medico medico);
        Task<Medico?> ObterPorIdAsync(int id);
        Task<List<Medico>> GetAll(string? especialidade);
     }
}
