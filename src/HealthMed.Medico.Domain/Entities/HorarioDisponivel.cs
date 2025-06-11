using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Domain.Entities
{
    public class HorarioDisponivel
    {

        public int Id { get; set; }
        public int MedicoId { get; set; }
        public DateTime DataHora { get; set; }
        public bool Ocupado { get; set; }

        public decimal ValorConsulta { get; set; }
        public Medico Medico { get; set; } = null!;
    }
}
