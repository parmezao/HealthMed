using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Domain.Validations
{
  public  class MedicoValidations
    {
        public static bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        public static bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }
    }
}
