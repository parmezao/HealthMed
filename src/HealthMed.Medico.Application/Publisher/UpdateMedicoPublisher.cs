using HealthMed.Doctor.Application.Events;
using HealthMed.Doctor.Application.ViewModels;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Application.Publisher
{
  public  class UpdateMedicoPublisher
    {
        private readonly IBus _bus;

        public UpdateMedicoPublisher(IBus bus)
        {
            _bus = bus;
        }

        public async Task PublishContactAsync(UpdateMedicoRequest request)
        {
            var updateEvent = new UpdateMedicoEvent
            {
                Id = request.Id,
                Nome = request.Nome,
                Especialidade = request.Especialidade,
                CRM = request.CRM,
                Horarios = request.Horarios
            };

            await _bus.Publish(updateEvent);
        }
    }
}
