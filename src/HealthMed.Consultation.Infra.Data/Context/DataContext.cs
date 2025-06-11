using HealthMed.Consultation.Domain.Core;
using HealthMed.Consultation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace HealthMed.Consultation.Infra.Data.Context
{
    public class DataContext : DbContext, IUnitOfWork
    {
        public DbSet<Consulta> Consultas => Set<Consulta>();

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Consulta>(ConfigureConsulta);
        }

        private void ConfigureConsulta(EntityTypeBuilder<Consulta> entity)
        {
            entity.ToTable("consultas");

            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasColumnName("id").HasColumnType("integer");
            entity.Property(c => c.CpfPaciente).HasColumnName("cpf_paciente").HasMaxLength(11).IsRequired();
            entity.Property(c => c.NomePaciente).HasColumnName("nome_paciente").HasMaxLength(255).IsRequired();
            entity.Property(c => c.CrmMedico).HasColumnName("crm_medico").HasMaxLength(50).IsRequired();
            entity.Property(c => c.DataHora).HasColumnName("data_hora").IsRequired().HasConversion(
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc), // para o banco
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)); // do banco;
            entity.Property(c => c.Status).HasColumnName("status").HasMaxLength(50).HasDefaultValue("Pendente").IsRequired();
            entity.Property(c => c.Justificativa).HasColumnName("justificativa").HasMaxLength(255).IsRequired(false);
        }


        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}
