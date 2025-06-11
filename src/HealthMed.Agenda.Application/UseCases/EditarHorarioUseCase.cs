using HealthMed.Agenda.Application.Events;
using HealthMed.Agenda.Application.Interfaces;
using HealthMed.Agenda.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Agenda.Application.UseCases
{
    public class EditarHorarioUseCase([FromServices] IAgendaPublisher agendaPublisher) : IEditarHorarioUseCase
    {
        public async Task<PublishResponse> ExecuteAsync(EditarHorarioRequest editarHorarioRequest)
        {
            //TODO: Validar se horário existe na base

            await agendaPublisher.PublishEditarHorarioAsync(new EditarHorarioEvent
            {
                Id = editarHorarioRequest.Id,
                DataHora = editarHorarioRequest.DataHora,
               
                
            });

            return new PublishResponse
            {
                Message = "Alteração em processamento.",
                Data = new
                {
                    editarHorarioRequest.Id,
                    editarHorarioRequest.DataHora
                }
            };
        }
    }
}


