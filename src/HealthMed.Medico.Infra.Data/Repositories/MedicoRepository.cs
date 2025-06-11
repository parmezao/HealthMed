using HealthMed.Doctor.Domain.Core;
using HealthMed.Doctor.Domain.Entities;
using HealthMed.Doctor.Domain.Interfaces;
using HealthMed.Doctor.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Infra.Data.Repositories
{
    public class MedicoRepository(DataContext dataContext) : IMedicoRepository
    {
        public IUnitOfWork UnitOfWork => dataContext;



        public void Dispose()
        {
            dataContext.Dispose();
            GC.SuppressFinalize(this);
        }


        public async Task<Medico> ObterPorCrmAsync(string crm)
        {
            return await dataContext.Medicos
           .Include(m => m.Horarios)
           .FirstOrDefaultAsync(m => m.CRM == crm);
        }

        public void Save(Medico medico)
        {
            dataContext.Medicos.Add(medico);
        }

        public void Update(Medico medico)
        {
            dataContext.Medicos.Update(medico);
        }

        public void Delete(Medico medico)
        {

            dataContext.Medicos.Remove(medico);
        }

        public async Task<Medico?> ObterPorIdAsync(int id)
        {
            return await dataContext.Medicos.Include(m => m.Horarios).FirstOrDefaultAsync(x => x.Id  ==  id);
        }

        public async Task<List<Medico>> GetAll(string? especialidade)
        {
            return await dataContext.Medicos
                .Where(m => m.Especialidade.Contains(especialidade) || string.IsNullOrEmpty(especialidade))
                .Include(m => m.Horarios)
                .OrderBy(x => x.Nome)
                .ToListAsync();
        }
    }
}
