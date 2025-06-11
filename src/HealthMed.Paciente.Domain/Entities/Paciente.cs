using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HealthMed.Patient.Domain.Entities
{
   public class Paciente
    {
        public int Id { get; set; }  
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public Paciente() { }

        public Paciente(string nome,string cpf, string email)
        {
            Validar(nome, cpf, email);
            Nome = nome;
            Cpf = cpf;
            Email = email;
        }

        public void Update(string nome, string cpf, string email)
        {
            Validar(nome, cpf, email);
            Nome = nome;
            Cpf = cpf;
            Email = email;
        }

        private void Validar(string nome, string cpf, string email)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome é obrigatório.");

            if (nome.Length > 255)
                throw new ArgumentException("O nome deve ter no máximo 255 caracteres.");

            if (!Regex.IsMatch(cpf ?? "", @"^\d{11}$"))
                throw new ArgumentException("O CPF deve conter 11 dígitos numéricos.");

            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[\w\.-]+@[\w\.-]+\.\w{2,4}$"))
                throw new ArgumentException("O e-mail informado é inválido.");
        }
    }
}
