using HealthMed.Consultation.Domain.Core;
using HealthMed.Consultation.Domain.Entities;
using HealthMed.Consultation.Domain.Interfaces;
using HealthMed.Consultation.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Consultation.Infra.Data.Repositories
{
    public class ConsultaRepository(DataContext dataContext) : IConsultaRepository
    {

        public IUnitOfWork UnitOfWork => dataContext;

        public void Dispose()
        {
            dataContext.Dispose();
            GC.SuppressFinalize(this);
        }
        public async Task AgendarAsync(Consulta consulta)
        {
            dataContext.Consultas.Add(consulta);
        }

        public async Task AtualizarStatusAsync(Consulta consulta)
        {
            
            dataContext.Consultas.Update(consulta);

        }

        public async Task<List<Consulta>> ObterPorCpfAsync(string cpf)
        {
            return await dataContext.Consultas.Where(x => x.CpfPaciente == cpf).ToListAsync();
        }

        public async Task<List<Consulta>> ObterPorCrmAsync(string crm)
        {
            return await dataContext.Consultas.Where(x => x.CrmMedico == crm).ToListAsync();
        }

        public async Task< Consulta > ObterPorIdAsync(int Id)
        {
            return await dataContext.Consultas.FirstOrDefaultAsync(x => x.Id == Id);
        }
    }
}
