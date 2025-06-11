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
    public class EspecialidadeRepository(DataContext dataContext) : IEspecialidadeRepository
    {
        public IUnitOfWork UnitOfWork => dataContext;



        public void Dispose()
        {
            dataContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<Especialidade?> GetByCategoria(string Categoria)
        {
            return await dataContext.Especialidades.FirstOrDefaultAsync(x => x.Categoria == Categoria);
        }

        public async Task<Especialidade?> GetByNome(string Nome)
        {
            return await dataContext.Especialidades.FirstOrDefaultAsync(x => x.Nome == Nome);
        }

        public async Task<List<Especialidade>> ListarTodasAsync()
        {
            return await dataContext.Especialidades.OrderBy(x => x.Categoria).ToListAsync();
        }
    }
}
