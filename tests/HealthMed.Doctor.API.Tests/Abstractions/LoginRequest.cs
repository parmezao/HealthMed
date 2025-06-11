using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.API.Tests.Abstractions
{
    public class LoginRequest
    {
        public string Login { get; set; } // CRM ou Email ou CPF
        public string Senha { get; set; }
    }
}
