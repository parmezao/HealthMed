using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Consultation.Application.ViewModels
{
    public record AgendarConsultaRequest(string CpfPaciente, string NomePaciente, string CrmMedico, DateTime DataHora,string status,string justificativa);
}
