using HealthMed.Consultation.Application.Events;
using HealthMed.Consultation.Application.Interfaces;
using HealthMed.Consultation.Application.ViewModels;
using HealthMed.Consultation.Domain.Validations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Consultation.Application.UseCases
{
    public class AgendarConsultaUseCase([FromServices] IConsultaPublisher consultaPublisher /* Serviço dedicado para publicar */) : IAgendarConsultaUseCase
    {
        public async Task<PublishResponse> Execute(AgendarConsultaRequest agendarConsultaRequest)
        {
            if (!ConsultaValidator.IsValidNome(agendarConsultaRequest.NomePaciente))
                throw new ArgumentException("O nome é obrigatório.");

            if (!ConsultaValidator.IsValidCRM(agendarConsultaRequest.CrmMedico))
                throw new ArgumentException($"O crm do médico é obrigatório.");

            // Cria a mensagem e publica na fila
            await consultaPublisher.PublishAgendarConsultaAsync(new   AgendarConsultaEvent
            {
                CpfPaciente = agendarConsultaRequest.CpfPaciente,
                NomePaciente = agendarConsultaRequest.NomePaciente,
                CrmMedico = agendarConsultaRequest.CrmMedico,
                DataHora = agendarConsultaRequest.DataHora,
                Status  = agendarConsultaRequest.status,
                Justificativa = agendarConsultaRequest.justificativa
            });

            return new PublishResponse
            {
                Message = "Cadastro em processamento.",
                Data = new
                {
                    agendarConsultaRequest.NomePaciente,
                    agendarConsultaRequest.CpfPaciente,
                    agendarConsultaRequest.CrmMedico
                }
            };
        }
    }
}
