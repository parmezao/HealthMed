using HealthMed.Agenda.Domain.Core;
using HealthMed.Agenda.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.Agenda.Infra.Data.Context
{
    public class DataContext : DbContext, IUnitOfWork
    {
        public DbSet<HorarioDisponivel> HorariosDisponiveis => Set<HorarioDisponivel>();

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HorarioDisponivel>(ConfigureHorario);
        }
        private void ConfigureHorario(EntityTypeBuilder<HorarioDisponivel> entity)
        {
            entity.ToTable("horarios_disponiveis");

            entity.HasKey(h => h.Id);
            entity.Property(h => h.Id).HasColumnName("id").HasColumnType("integer");
            entity.Property(h => h.MedicoId).HasColumnName("medico_id").IsRequired();
            entity.Property(h => h.DataHora).HasColumnName("data_hora").IsRequired().HasConversion(
            v => v, // para o banco
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)); // do banco;
            
            entity.Property(h => h.Ocupado).HasColumnName("ocupado").HasDefaultValue(false);
            entity.Property(h => h.ValorConsulta)
            .HasColumnName("valor_consulta")
            .HasColumnType("decimal(10,2)")
            .IsRequired(); // se for NOT NULL
        }
        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}
