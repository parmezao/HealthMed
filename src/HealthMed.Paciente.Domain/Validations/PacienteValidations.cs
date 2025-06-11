using System.Text.RegularExpressions;

namespace HealthMed.Patient.Domain.Validations
{
    public  class PacienteValidations
    {
        public static bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        public static bool IsValidNome(string nome)
        {
            return !string.IsNullOrWhiteSpace(nome);
        }
    }
}
