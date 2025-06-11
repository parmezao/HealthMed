using HealthMed.Doctor.Application.Events;
using HealthMed.Doctor.Application.Interfaces;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Application.UseCases
{
 public   class MedicoPublisher : IMedicoPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MedicoPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

       

        public async Task PublishInsertMedicoAsync(InsertMedicoEvent message)
        {
            await _publishEndpoint.Publish(message);

        }

        public async Task PublishUpdateMedicoAsync(UpdateMedicoEvent message)
        {
            await _publishEndpoint.Publish(message);

        }

        public async Task PublishDeleteMedicotAsync(DeleteMedicoEvent message)
        {
            await _publishEndpoint.Publish(message);

        }
    }
}
