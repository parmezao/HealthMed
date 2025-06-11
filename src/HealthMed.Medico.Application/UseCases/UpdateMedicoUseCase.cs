using HealthMed.Doctor.Application.Events;
using HealthMed.Doctor.Application.Interfaces;
using HealthMed.Doctor.Application.ViewModels;
using HealthMed.Doctor.Domain.Entities;
using HealthMed.Doctor.Domain.Interfaces;
using HealthMed.Doctor.Domain.Validations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Application.UseCases
{
    public class UpdateMedicoUseCase([FromServices] IMedicoPublisher medicoPublisher /* Serviço dedicado para publicar */,
        IMedicoRepository medicoRepository) : IUpdateMedicoUseCase
    {
        
        public async Task<PublishResponse> Execute(UpdateMedicoRequest updateMedicoRequest)
        {
            var contact = await medicoRepository.ObterPorIdAsync(updateMedicoRequest.Id);
            if (contact == null) throw new ApplicationException("Não foi possível localizar o cadastro do médico informado.");

            if (!MedicoValidations.IsValidName(updateMedicoRequest.Nome))
                throw new ArgumentException("O nome é obrigatório.");



            // Mapeia os horários
            var horarios = updateMedicoRequest.Horarios?.Select(h => new HorarioDisponivel
            {
                DataHora = h.DataHora,
                Ocupado = h.Ocupado
            }).ToList() ?? new();


            var medico = new Medico(updateMedicoRequest.Nome, updateMedicoRequest.Especialidade, updateMedicoRequest.CRM, horarios);

            // Cria a mensagem e publica na fila
            await medicoPublisher.PublishUpdateMedicoAsync(new UpdateMedicoEvent
            {
                Id = updateMedicoRequest.Id,
                Nome = updateMedicoRequest.Nome,
                Especialidade = updateMedicoRequest.Especialidade,
                CRM = updateMedicoRequest.CRM,
                Horarios = updateMedicoRequest.Horarios
            });

            return new PublishResponse
            {
                Message = "Atualização em processamento.",
                Data = new
                {
                    updateMedicoRequest.Nome,
                    updateMedicoRequest.Especialidade,
                    updateMedicoRequest.CRM
                }
            };
        }
    }
}
