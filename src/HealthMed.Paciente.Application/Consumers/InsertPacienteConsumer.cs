using HealthMed.Patient.Application.Events;
using HealthMed.Patient.Domain.Entities;
using HealthMed.Patient.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.Patient.Application.Consumers
{
    public class InsertPacienteConsumer : IConsumer<InsertPacienteEvent>
    {
        private readonly IServiceProvider _serviceProvider;
        public InsertPacienteConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task Consume(ConsumeContext<InsertPacienteEvent> context)
        {
            var message = context.Message;

            try
            {
                var paciente = new Paciente(message.Nome, message.Cpf, message.Email);

                // Grava Paciente no DB
                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopedProcessingService =
                        scope.ServiceProvider
                        .GetRequiredService<IPacienteRepository>();

                    await scopedProcessingService.AdicionarAsync(paciente);
                    await scopedProcessingService.UnitOfWork.Commit();
                }

                // TODO: Remover
                //System.Threading.Thread.Sleep(10000);

                Console.WriteLine($"Paciente inserido com sucesso: {paciente.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem:Email:{message.Email} {ex.Message}");
            }
        }
    }
}
