using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Consultation.Application.Events
{
  public  class AgendarConsultaEvent
    {
     
        public string CpfPaciente { get; set; } 
        public string NomePaciente { get; set; } 
        public string CrmMedico { get; set; } 
        public DateTime DataHora { get; set; }
        public string Status { get; set; }
        public string? Justificativa { get; set; }
    }
}
