using HealthMed.Agenda.Application.Events;
using HealthMed.Agenda.Domain.Entities;
using HealthMed.Agenda.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.Agenda.Application.Consumers
{
    public class CadastrarHorarioConsumer : IConsumer<CadastrarHorarioEvent>
    {
        private readonly IServiceProvider _serviceProvider;
        public CadastrarHorarioConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task Consume(ConsumeContext<CadastrarHorarioEvent> context)
        {
            var message = context.Message;

            try
            {
                var horarioDisponivel = new HorarioDisponivel(message.MedicoId, message.DataHora, false, message.ValorConsulta);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopedProcessingService =
                        scope.ServiceProvider
                        .GetRequiredService<IAgendaRepository>();

                    await scopedProcessingService.AdicionarAsync(horarioDisponivel);
                    await scopedProcessingService.UnitOfWork.Commit();
                }

                // TODO: Remover
                //System.Threading.Thread.Sleep(10000);

                Console.WriteLine($"Horário inserido com sucesso: {horarioDisponivel.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem:MedicoId:{message.MedicoId}:{message.DataHora} {ex.Message}");
            }
        }
    }
}
