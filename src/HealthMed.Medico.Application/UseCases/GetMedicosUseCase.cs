using HealthMed.Doctor.Application.Interfaces;
using HealthMed.Doctor.Application.ViewModels;
using HealthMed.Doctor.Domain.Entities;
using HealthMed.Doctor.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Application.UseCases
{
    public class GetMedicosUseCase(IMedicoRepository medicoRepository) : IGetMedicosUseCase
    {
        public async Task<List<MedicoResponse>> GetAll(string? especialidade)
        {
            var result = await medicoRepository.GetAll(especialidade);
            return result.Select(MapToResponse).ToList();
        }

        public async Task<MedicoResponse> ObterPorCrmAsync(string crm)
        {
            var medico = await medicoRepository.ObterPorCrmAsync(crm);

            if (medico is null)
                throw new ApplicationException("Médico não encontrado!");

            return MapToResponse(medico);
        }

        public async Task<MedicoResponse?> ObterPorIdAsync(int id)
        {

            var medico = await medicoRepository.ObterPorIdAsync(id);

            if (medico is null)
                throw new ApplicationException("Médico não encontrado!");

            return MapToResponse(medico);
        }

        private static MedicoResponse MapToResponse(Medico medico)
        {
            return new MedicoResponse(
                medico.Id,
                medico.Nome,
                medico.Especialidade,
                medico.CRM,
                medico.Horarios.Select(h => new HorarioDto
                {
                    DataHora = h.DataHora,
                    Ocupado = h.Ocupado ,
                    ValorConsulta = h.ValorConsulta
                }).ToList()
            );
        }

    }
}
