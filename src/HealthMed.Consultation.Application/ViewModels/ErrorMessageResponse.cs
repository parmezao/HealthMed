using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthMed.Consultation.Application.ViewModels
{
    public record ErrorMessageResponse([property: JsonPropertyName("message")] string Message);
}
