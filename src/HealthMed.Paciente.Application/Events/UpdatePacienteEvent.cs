using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HealthMed.Patient.Application.Events
{
 public   class UpdatePacienteEvent
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }

        public UpdatePacienteEvent()
        {

        }

        public UpdatePacienteEvent(int id, string nome, string cpf, string email)
        {
            Id = id;
            Nome  = nome;
            Cpf = cpf;
            Email = email;
        }
    }
}
