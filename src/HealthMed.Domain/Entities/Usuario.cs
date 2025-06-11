using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Auth.Domain.Entities;

public abstract class Usuario
{
    public int Id { get; set; }  
    public string Nome { get; set; }
    public string SenhaHash { get; set; }
    public string? Cpf { get; set; }
    public string? Email { get; set; }
    public string? Crm { get; set; }


    // ✅ Expor o papel (role) com base no tipo concreto
    public string Role => GetType().Name.ToLower(); // medico, paciente
}


