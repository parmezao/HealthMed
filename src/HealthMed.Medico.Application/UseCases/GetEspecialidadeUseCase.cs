using HealthMed.Doctor.Application.Interfaces;
using HealthMed.Doctor.Application.ViewModels;
using HealthMed.Doctor.Domain.Entities;
using HealthMed.Doctor.Domain.Interfaces;
using MassTransit.Internals.GraphValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Application.UseCases
{
    public class GetEspecialidadeUseCase(IEspecialidadeRepository especialidadeRepository) : IGetEspecialidadeUseCase
    {
        public async Task<EspecialidadeResponse?> GetByCategoria(string Categoria)
        {
            var espec = await especialidadeRepository.GetByCategoria(Categoria);

            if (espec is null)
                throw new ApplicationException("Especialidade não encontrado!");

            return MapToResponse(espec);
        }

        public async Task<EspecialidadeResponse?> GetByNome(string Nome)
        {
            var espec = await especialidadeRepository.GetByNome(Nome);

            if (espec is null)
                throw new ApplicationException("Especialidade não encontrado!");

            return MapToResponse(espec);
        }

        public async Task<List<EspecialidadeResponse>> ListarTodasAsync()
        {
            var result = await especialidadeRepository.ListarTodasAsync();
            return result.Select(MapToResponse).ToList();
        }

        private static EspecialidadeResponse MapToResponse(Especialidade especialidade)
        {
            return new EspecialidadeResponse(
                especialidade.Id,
                especialidade.Nome,
                especialidade.Categoria
                 ) ;
           
        }
    }
}
