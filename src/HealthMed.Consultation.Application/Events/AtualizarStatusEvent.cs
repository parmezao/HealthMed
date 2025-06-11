using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Consultation.Application.Events
{
   public class AtualizarStatusEvent
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string? Justificativa { get; set; }
   
    }
}
