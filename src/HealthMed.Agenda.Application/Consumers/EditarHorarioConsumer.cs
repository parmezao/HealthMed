using HealthMed.Agenda.Application.Events;
using HealthMed.Agenda.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Agenda.Application.Consumers
{
  public  class EditarHorarioConsumer : IConsumer<EditarHorarioEvent>
    {
        private readonly IServiceProvider _serviceProvider;
        public EditarHorarioConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Consume(ConsumeContext<EditarHorarioEvent> context)
        {
            var message = context.Message;

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopedProcessingService =
                        scope.ServiceProvider
                        .GetRequiredService<IAgendaRepository>();

                    var horarioDisponivel = await scopedProcessingService.ObterPorIdAsync(message.Id);

                    horarioDisponivel.Update(message.DataHora);
                    await scopedProcessingService.AtualizarAsync(horarioDisponivel);
                    await scopedProcessingService.UnitOfWork.Commit();

                    // TODO: Remover
                    //System.Threading.Thread.Sleep(10000);

                    Console.WriteLine($"Horário disponível atualizado com sucesso: {horarioDisponivel.Id}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem: Email: {message.Id}:{message.DataHora} {ex.Message}");
            }
        }
 
    }
}
