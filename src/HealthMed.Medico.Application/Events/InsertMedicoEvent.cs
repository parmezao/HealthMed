using HealthMed.Doctor.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Application.Events
{
public    class InsertMedicoEvent
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Especialidade { get; set; } = string.Empty;
        public string CRM { get; set; } = string.Empty;
        public List<HorarioDto> Horarios { get; set; } = new();
    }
}
