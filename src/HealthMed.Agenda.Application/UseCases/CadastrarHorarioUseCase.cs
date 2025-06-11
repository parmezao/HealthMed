using HealthMed.Agenda.Application.Events;
using HealthMed.Agenda.Application.Interfaces;
using HealthMed.Agenda.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Agenda.Application.UseCases
{
    public class CadastrarHorarioUseCase([FromServices] IAgendaPublisher agendaPublisher) : ICadastrarHorarioUseCase 
    {
        public async Task<PublishResponse> ExecuteAsync(CadastrarHorarioRequest cadastrarHorarioRequest)
        {
            //TODO: Validar se horário já foi cadastrado e se médico existente

            await agendaPublisher.PublishCadastrarHorarioAsync(new CadastrarHorarioEvent
            {
                MedicoId = cadastrarHorarioRequest.MedicoId,
                DataHora = cadastrarHorarioRequest.DataHora,
                ValorConsulta = cadastrarHorarioRequest.ValorConsulta
            });

            return new PublishResponse
            {
                Message = "Cadastro em processamento.",
                Data = new
                {
                    cadastrarHorarioRequest.MedicoId,
                    cadastrarHorarioRequest.DataHora,
                    cadastrarHorarioRequest.ValorConsulta
                }
            };
        }
    }
}

 
