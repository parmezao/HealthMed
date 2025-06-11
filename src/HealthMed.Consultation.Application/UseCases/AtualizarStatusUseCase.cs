using HealthMed.Consultation.Application.Events;
using HealthMed.Consultation.Application.Interfaces;
using HealthMed.Consultation.Application.ViewModels;
using HealthMed.Consultation.Domain.Interfaces;
using HealthMed.Consultation.Domain.Validations;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Consultation.Application.UseCases
{
    public class AtualizarStatusUseCase([FromServices] IConsultaPublisher consultaPublisher /* Serviço dedicado para publicar */,IConsultaRepository consultaRepository) : IAtualizarStatusUseCase
    {
        public async Task<PublishResponse> Execute(AtualizarStatusRequest atualizarStatusRequest)
        {
            var consulta = await consultaRepository.ObterPorIdAsync(atualizarStatusRequest.ConsultaId);
            if (consulta == null) throw new ApplicationException("Não foi possível localizar o cadastro da consulta informada.");

            if (!ConsultaValidator.IsValidStatus(atualizarStatusRequest.NovoStatus))
                throw new ArgumentException("O Status é obrigatório.");

            if (ConsultaValidator.IsJustifiedStatusAndEmptyJustify(atualizarStatusRequest.NovoStatus, atualizarStatusRequest.Justificativa))
                throw new ArgumentException("Justificativa é obrigatório.");

            await consultaPublisher.PublishAtualizarStatusAsync(new  AtualizarStatusEvent
            {
                Id = atualizarStatusRequest.ConsultaId,
                Status = atualizarStatusRequest.NovoStatus,
                Justificativa = atualizarStatusRequest.Justificativa,
                
            });

            return new PublishResponse
            {
                Message = "Atualização em processamento.",
                Data = new
                {
                    atualizarStatusRequest.ConsultaId,
                    atualizarStatusRequest.NovoStatus,
                    atualizarStatusRequest.Justificativa 
                }
            };
        }
    }
}
