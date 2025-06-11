using HealthMed.Consultation.Application.Events;
using HealthMed.Consultation.Domain.Entities;
using HealthMed.Consultation.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Consultation.Application.Consumers
{

    public class AgendarConsultaConsumer : IConsumer<AgendarConsultaEvent>
    {
        private readonly IServiceProvider _serviceProvider;
        public AgendarConsultaConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task Consume(ConsumeContext<AgendarConsultaEvent> context)
        {
            var message = context.Message;

            try
            {
                var consulta = new Consulta( message.CpfPaciente, message.NomePaciente, message.CrmMedico, message.DataHora, message.Status,message.Justificativa);

                // Grava Paciente no DB
                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopedProcessingService =
                        scope.ServiceProvider
                        .GetRequiredService<IConsultaRepository>();


                    await scopedProcessingService.AgendarAsync(consulta);
                    await scopedProcessingService.UnitOfWork.Commit();
                }

                

                Console.WriteLine($"Paciente inserido com sucesso: {consulta.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem:NomePaciente:{message.NomePaciente} {ex.Message}");
            }
        }
    }
}
