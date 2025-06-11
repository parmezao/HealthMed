using HealthMed.Agenda.Application.Events;
using HealthMed.Agenda.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.Agenda.Application.Consumers
{
    public class RemoverHorarioConsumer : IConsumer<RemoverHorarioEvent>
    {
        private readonly IServiceProvider _serviceProvider;
        public RemoverHorarioConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Consume(ConsumeContext<RemoverHorarioEvent> context)
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

                    await scopedProcessingService.RemoverAsync(horarioDisponivel);
                    await scopedProcessingService.UnitOfWork.Commit();

                    // TODO: Remover
                    //System.Threading.Thread.Sleep(10000);

                    Console.WriteLine($"Horário disponível deletado com sucesso: {horarioDisponivel.Id}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem: Id:{message.Id} {ex.Message}");
            }
        }
  
    }
}
