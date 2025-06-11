
using HealthMed.Patient.Domain.Core;
using HealthMed.Patient.Domain.Entities;
using HealthMed.Patient.Domain.Interfaces;
using HealthMed.Patient.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Patient.Infra.Data.Repositories
{
    public class PacienteRepository(DataContext dataContext) : IPacienteRepository
    {
        public IUnitOfWork UnitOfWork => dataContext;

        public void Dispose()
        {
            dataContext.Dispose();
            GC.SuppressFinalize(this);
        }
        public async Task AdicionarAsync(Paciente paciente)
        {
            dataContext.Pacientes.Add(paciente);
        }

        public async Task AtualizarAsync(Paciente paciente)
        {
            dataContext.Pacientes.Update(paciente);
        }

        public async Task<Paciente?> ObterPorCpfAsync(string cpf)
        {
            return await dataContext.Pacientes.FirstOrDefaultAsync(x => x.Cpf ==  cpf);
        }

        public async Task<Paciente?> ObterPorIdAsync(int id)
        {
            return await dataContext.Pacientes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Paciente>> ObterTodosAsync()
        {
            return await dataContext.Pacientes.OrderBy(x => x.Nome).ToListAsync();
        }

        public async Task RemoverAsync(Paciente paciente)
        {
              dataContext.Pacientes.Remove(paciente);
        }
    }
}
