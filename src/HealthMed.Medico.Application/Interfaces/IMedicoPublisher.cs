using HealthMed.Doctor.Application.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Application.Interfaces
{
public    interface IMedicoPublisher
    {
        Task PublishInsertMedicoAsync(InsertMedicoEvent message);
        Task PublishUpdateMedicoAsync(UpdateMedicoEvent message);
        Task PublishDeleteMedicotAsync(DeleteMedicoEvent message);
    }
}
