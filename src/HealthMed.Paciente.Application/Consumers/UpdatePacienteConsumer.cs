using HealthMed.Patient.Application.Events;
using HealthMed.Patient.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.Patient.Application.Consumers
{
  public  class UpdatePacienteConsumer : IConsumer<UpdatePacienteEvent>
    {
        private readonly IServiceProvider _serviceProvider;
        public UpdatePacienteConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Consume(ConsumeContext<UpdatePacienteEvent> context)
        {
            var message = context.Message;

            try
            {
                // Grava contato no DB
                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopedProcessingService =
                        scope.ServiceProvider
                        .GetRequiredService<IPacienteRepository>();

                    var paciente = await scopedProcessingService.ObterPorIdAsync(message.Id);

                    paciente.Update(message.Nome, message.Cpf, message.Email);
                    await scopedProcessingService.AtualizarAsync(paciente);
                    await scopedProcessingService.UnitOfWork.Commit();

                    // TODO: Remover
                    //System.Threading.Thread.Sleep(10000);

                    Console.WriteLine($"Paciente atualizado com sucesso: {paciente.Id}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem: Email:{message.Email} {ex.Message}");
            }
        }
 
    }
}
