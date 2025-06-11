using HealthMed.Doctor.Application.Events;
using HealthMed.Doctor.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Application.Consumers
{
 public   class DeleteMedicoConsumer : IConsumer<DeleteMedicoEvent>
    {
        private readonly IServiceProvider _serviceProvider;
        public DeleteMedicoConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Consume(ConsumeContext<DeleteMedicoEvent> context)
        {
            var message = context.Message;

            try
            {
                // Deleta Médico no DB
                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopedProcessingService =
                        scope.ServiceProvider
                        .GetRequiredService<IMedicoRepository>();

                    var med = await scopedProcessingService.ObterPorIdAsync(message.Id);

                    scopedProcessingService.Delete(med);
                    await scopedProcessingService.UnitOfWork.Commit();

                    // TODO: Remover
                    //System.Threading.Thread.Sleep(10000);

                    Console.WriteLine($"Médico deletado com sucesso: {med.Id}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem: Id:{message.Id} {ex.Message}");
            }
        }
    }
}

 
