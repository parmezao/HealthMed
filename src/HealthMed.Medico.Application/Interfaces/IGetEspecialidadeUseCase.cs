using HealthMed.Doctor.Application.ViewModels;
using HealthMed.Doctor.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Application.Interfaces
{
  public  interface IGetEspecialidadeUseCase
    {
        Task<List<EspecialidadeResponse>> ListarTodasAsync();
        Task<EspecialidadeResponse?> GetByCategoria(string Categoria);
        Task<EspecialidadeResponse?> GetByNome(string Nome);
    }
}
