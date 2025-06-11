using HealthMed.Agenda.Application.Events;
using HealthMed.Agenda.Application.Interfaces;
using HealthMed.Agenda.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Agenda.Application.UseCases
{
    public class RemoverHorarioUseCase([FromServices] IAgendaPublisher agendaPublisher) : IRemoverHorarioUseCase
    {
        public async Task<PublishResponse> ExecuteAsync(RemoverHorarioRequest removerHorarioRequest)
        {
            //TODO: Validar se horário existe na base

            await agendaPublisher.PublishRemoverHorarioAsync(new RemoverHorarioEvent
            {
                Id = removerHorarioRequest.Id
            });

            return new PublishResponse
            {
                Message = "Exclusão em processamento.",
                Data = new
                {
                    removerHorarioRequest.Id
                }
            };
        }
    }
}


