using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Domain.Entities
{
    public class Medico
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Especialidade { get; private set; }
        public string CRM { get; private set; }
        public List<HorarioDisponivel> Horarios { get; set; } = new();

        // Construtor para EF Core
        public Medico() { }

        // Construtor de domínio
        public Medico(string nome, string especialidade, string crm, List<HorarioDisponivel>? horarios = null)
        {
            Validar(nome, especialidade, crm);
         
            Nome = nome;
            Especialidade = especialidade;
            CRM = crm;
            Horarios = horarios ?? new List<HorarioDisponivel>();
        }

        public void AtualizarHorarios(List<DateTime> novosHorarios)
        {
            Horarios = novosHorarios.Select(dh => new HorarioDisponivel { DataHora = dh  }).ToList();
        }

        public void Update(string nome, string especialidade, string crm, List<HorarioDisponivel>? horarios = null)
        {
            Validar(nome, especialidade, crm);

            Nome = nome;
            Especialidade = especialidade;
            CRM = crm;
            Horarios = horarios ?? new List<HorarioDisponivel>();
        }

        private void Validar(string nome, string especialidade, string crm)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do médico é obrigatório.");

            if (string.IsNullOrWhiteSpace(especialidade))
                throw new ArgumentException("Especialidade é obrigatória.");

            if (string.IsNullOrWhiteSpace(crm))
                throw new ArgumentException("CRM é obrigatório.");
        }
    }

}
