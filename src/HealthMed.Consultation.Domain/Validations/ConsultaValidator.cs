using System.Text.RegularExpressions;

namespace HealthMed.Consultation.Domain.Validations
{
    public   class ConsultaValidator
    {
        private static readonly string[] StatusComJustificativaObrigatoria = new[] { "Cancelada", "Recusada" };
        public static bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        public static bool IsValidStatus(string status)
        {
            return !string.IsNullOrWhiteSpace(status);
        }

        public static bool IsJustifiedStatusAndEmptyJustify(string status, string? justificativa)
        {
            return StatusComJustificativaObrigatoria.Contains(status) && string.IsNullOrEmpty(justificativa);
        }

        public static bool IsValidNome(string Nome)
        {
            return !string.IsNullOrWhiteSpace(Nome);
        }

        public static bool IsValidCRM(string crm)
        {
            return !string.IsNullOrWhiteSpace(crm);
        }
    }
}
