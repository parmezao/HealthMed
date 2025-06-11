using HealthMed.Consultation.Application.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Consultation.Application.Interfaces
{
 public   interface IConsultaPublisher
    {
        Task PublishAgendarConsultaAsync(AgendarConsultaEvent message);
        Task PublishAtualizarStatusAsync(AtualizarStatusEvent message);
    }
}
