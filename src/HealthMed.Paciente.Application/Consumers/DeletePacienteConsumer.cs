using HealthMed.Patient.Application.Events;
using HealthMed.Patient.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Patient.Application.Consumers
{
   public class DeletePacienteConsumer : IConsumer<DeletePacienteEvent>
    {
        private readonly IServiceProvider _serviceProvider;
        public DeletePacienteConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Consume(ConsumeContext<DeletePacienteEvent> context)
        {
            var message = context.Message;

            try
            {
                // Deleta contato no DB
                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopedProcessingService =
                        scope.ServiceProvider
                        .GetRequiredService<IPacienteRepository>();

                    var paciente = await scopedProcessingService.ObterPorIdAsync(message.Id);

                    await scopedProcessingService.RemoverAsync(paciente);
                    await scopedProcessingService.UnitOfWork.Commit();

                    // TODO: Remover
                    //System.Threading.Thread.Sleep(10000);

                    Console.WriteLine($"Paciente deletado com sucesso: {paciente.Id}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem: Id:{message.Id} {ex.Message}");
            }
        }
  
    }
}
