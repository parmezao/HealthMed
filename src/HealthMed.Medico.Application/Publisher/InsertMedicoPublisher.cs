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
  public  class InsertMedicoPublisher
    {
        private readonly IBus _bus;

        public InsertMedicoPublisher(IBus bus)
        {
            _bus = bus;
        }

        public async Task PublishContactAsync(InsertMedicoRequest request)
        {
            var contactEvent = new InsertMedicoEvent
            {
                Nome = request.Nome,
                Especialidade = request.Especialidade,
                CRM = request.CRM,
                Horarios = request.Horarios
            };

            await _bus.Publish(contactEvent);
        }
    }
}
