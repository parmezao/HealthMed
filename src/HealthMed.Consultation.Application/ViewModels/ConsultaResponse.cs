using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Consultation.Application.ViewModels
{
    public record ConsultaResponse(
                int Id,
                string CpfPaciente,
                string NomePaciente,
                string CrmMedico,
                DateTime DataHora,
                string Status,
                string? Justificativa
        );


}
