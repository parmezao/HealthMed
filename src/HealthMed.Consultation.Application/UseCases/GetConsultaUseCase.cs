using HealthMed.Consultation.Application.Interfaces;
using HealthMed.Consultation.Application.ViewModels;
using HealthMed.Consultation.Domain.Entities;
using HealthMed.Consultation.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Consultation.Application.UseCases
{
    public class GetConsultaUseCase(IConsultaRepository consultaRepository) : IGetConsultaUseCase
    {
        public async Task<List<ConsultaResponse>> ListarPorCpfAsync(string cpf)
        {
            List<Consulta> result = [];
            result = await consultaRepository.ObterPorCpfAsync(cpf);
            var mapped = result.Select(x => new ConsultaResponse(x.Id, x.CpfPaciente, x.NomePaciente, x.CrmMedico, x.DataHora,x.Status,x.Justificativa)).ToList();
            return mapped;
        }

        public async Task<List<ConsultaResponse>> ListarPorCrmAsync(string crm)
        {
            List<Consulta> result = [];
            result = await consultaRepository.ObterPorCrmAsync(crm);
            var mapped = result.Select(x => new ConsultaResponse(x.Id, x.CpfPaciente, x.NomePaciente, x.CrmMedico, x.DataHora, x.Status, x.Justificativa)).ToList();
            return mapped;
        }
    }
}
