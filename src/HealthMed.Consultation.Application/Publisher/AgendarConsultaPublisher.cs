using HealthMed.Consultation.Application.Events;
using HealthMed.Consultation.Application.ViewModels;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Consultation.Application.Publisher
{
  public  class AgendarConsultaPublisher
    {
        private readonly IBus _bus;

        public AgendarConsultaPublisher(IBus bus)
        {
            _bus = bus;
        }

        public async Task PublishContactAsync(AgendarConsultaRequest request)
        {
            var contactEvent = new AgendarConsultaEvent
            {
                CpfPaciente = request.CpfPaciente,
                NomePaciente = request.NomePaciente,
                CrmMedico = request.CrmMedico,
                DataHora = request.DataHora 

            };

            await _bus.Publish(contactEvent);
        }
    }
}
