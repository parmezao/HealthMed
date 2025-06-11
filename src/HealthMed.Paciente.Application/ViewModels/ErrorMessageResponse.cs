using System.Text.Json.Serialization;

namespace HealthMed.Patient.Application.ViewModels
{
    public record ErrorMessageResponse([property: JsonPropertyName("message")] string Message);
}
