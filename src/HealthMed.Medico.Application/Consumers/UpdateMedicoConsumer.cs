using HealthMed.Doctor.Application.Events;
using HealthMed.Doctor.Domain.Entities;
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
  public  class UpdateMedicoConsumer : IConsumer<UpdateMedicoEvent>
    {
        private readonly IServiceProvider _serviceProvider;
        public UpdateMedicoConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Consume(ConsumeContext<UpdateMedicoEvent> context)
        {
            var message = context.Message;

            try
            {
                // Grava Médico no DB
                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopedProcessingService =
                        scope.ServiceProvider
                        .GetRequiredService<IMedicoRepository>();


                    // Mapeia os horários
                    var horarios = message.Horarios?.Select(h => new HorarioDisponivel
                    {
                        DataHora = h.DataHora,
                        Ocupado = h.Ocupado
                    }).ToList() ?? new();


                    var medico = await scopedProcessingService.ObterPorIdAsync(message.Id);

                    medico.Update(message.Nome, message.Especialidade, message.CRM, horarios);
                    scopedProcessingService.Update(medico);
                    await scopedProcessingService.UnitOfWork.Commit();

                    // TODO: Remover
                    //System.Threading.Thread.Sleep(10000);

                    Console.WriteLine($"Médico Atualizado com sucesso: {medico.Nome} | CRM: {medico.CRM}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem:Nome:{message.Nome} {ex.Message}");
            }
        }
    }
}
