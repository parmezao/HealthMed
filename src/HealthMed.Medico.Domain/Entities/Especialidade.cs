using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Domain.Entities
{
    public class Especialidade
    {
        public int Id { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public string Categoria { get; private set; } = string.Empty;

        public Especialidade() { }

        public Especialidade(string nome, string categoria)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório.");
            if (string.IsNullOrWhiteSpace(categoria))
                throw new ArgumentException("Categoria é obrigatória.");

            Nome = nome;
            Categoria = categoria;
        }

        public Especialidade(int id ,string nome, string categoria)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório.");
            if (string.IsNullOrWhiteSpace(categoria))
                throw new ArgumentException("Categoria é obrigatória.");
            Id = id;
            Nome = nome;
            Categoria = categoria;
        }
    }

}
