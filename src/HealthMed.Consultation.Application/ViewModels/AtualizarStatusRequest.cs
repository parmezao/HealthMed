using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Consultation.Application.ViewModels
{
    public class AtualizarStatusRequest
    {
        public int ConsultaId { get; set; }
        public string NovoStatus { get; set; } = string.Empty;
        public string? Justificativa { get; set; }

    }
}
