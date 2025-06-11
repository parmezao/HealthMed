using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Application.ViewModels
{
  public  class HorarioDto
    {
        public DateTime DataHora { get; set; }
        public bool Ocupado { get; set; }
        public decimal ValorConsulta { get; set; }
    }
}
