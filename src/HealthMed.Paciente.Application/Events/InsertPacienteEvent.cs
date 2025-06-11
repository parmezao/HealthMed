using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Patient.Application.Events
{
    public class InsertPacienteEvent
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
    }
}
