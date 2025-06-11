using HealthMed.Doctor.Domain.Core;
using HealthMed.Doctor.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Domain.Interfaces
{
    public interface IEspecialidadeRepository : IRepository
    {
        Task<List<Especialidade>> ListarTodasAsync();
        Task<Especialidade?> GetByCategoria(string Categoria);
        Task<Especialidade?> GetByNome(string Nome);
    }
}
