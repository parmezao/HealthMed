using HealthMed.Doctor.Application.Events;
using HealthMed.Doctor.Application.Interfaces;
using HealthMed.Doctor.Application.ViewModels;
using HealthMed.Doctor.Domain.Entities;
using HealthMed.Doctor.Domain.Validations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Application.UseCases
{
    public class InsertMedicoUseCase([FromServices] IMedicoPublisher medicoPublisher /* Serviço dedicado para publicar */) : IInsertMedicoUseCase
    {
        public async Task<PublishResponse> Execute(InsertMedicoRequest insertMedicoRequest)
        {
            if (!MedicoValidations.IsValidName(insertMedicoRequest.Nome))
                throw new ArgumentException("O nome é obrigatório.");



            // Mapeia os horários
            var horarios = insertMedicoRequest.Horarios?.Select(h => new HorarioDisponivel
            {
                DataHora = h.DataHora,
                Ocupado = h.Ocupado,
                ValorConsulta = h.ValorConsulta
            }).ToList() ?? new();


            var medico = new Medico(insertMedicoRequest.Nome, insertMedicoRequest.Especialidade, insertMedicoRequest.CRM, horarios);

            // Cria a mensagem e publica na fila
            await medicoPublisher.PublishInsertMedicoAsync(new InsertMedicoEvent
            {
                Nome = insertMedicoRequest.Nome,
                Especialidade = insertMedicoRequest.Especialidade,
                CRM = insertMedicoRequest.CRM,
                Horarios = insertMedicoRequest.Horarios
            });

            return new PublishResponse
            {
                Message = "Cadastro em processamento.",
                Data = new
                {
                    insertMedicoRequest.Nome,
                    insertMedicoRequest.Especialidade,
                    insertMedicoRequest.CRM
                }
            };
        }
    }
}
