using HealthMed.Doctor.Application.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using HealthMed.Doctor.Domain.Interfaces;
using HealthMed.Doctor.Domain.Entities;
namespace HealthMed.Doctor.Application.Consumers
{

    public class InsertMedicoConsumer : IConsumer<InsertMedicoEvent>
    {
           private readonly IServiceProvider _serviceProvider;
        public InsertMedicoConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task Consume(ConsumeContext<InsertMedicoEvent> context)
        {
            var message = context.Message;

            try
            {

                // Mapeia os horários
                var horarios = message.Horarios?.Select(h => new HorarioDisponivel
                {
                    DataHora =  DateTime.SpecifyKind(h.DataHora, DateTimeKind.Unspecified) ,
                    Ocupado = h.Ocupado,
                    ValorConsulta = h.ValorConsulta
                    
                }).ToList() ?? new();

                var medico = new  Medico(message.Nome, message.Especialidade,message.CRM, horarios);

                // Grava Médico no DB
                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopedProcessingService =
                        scope.ServiceProvider
                        .GetRequiredService<IMedicoRepository>();


                    scopedProcessingService.Save(medico);
                    await scopedProcessingService.UnitOfWork.Commit();
                }

                // TODO: Remover
                //System.Threading.Thread.Sleep(10000);

                Console.WriteLine($"✅ Médico inserido com sucesso: {medico.Nome} | CRM: {medico.CRM}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem:Nome:{message.Nome} {ex.Message}");
            }
        }
    }
}
