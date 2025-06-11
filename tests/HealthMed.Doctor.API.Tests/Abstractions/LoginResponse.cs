using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.API.Tests.Abstractions
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public string Nome { get; set; }
    }
}
