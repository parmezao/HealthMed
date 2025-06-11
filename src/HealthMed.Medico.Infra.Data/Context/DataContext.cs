
using HealthMed.Doctor.Domain.Core;
using HealthMed.Doctor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Infra.Data.Context
{
    public class DataContext : DbContext, IUnitOfWork
    {
        public DbSet<Medico> Medicos { get; set; } = null!;
        public DbSet<Especialidade> Especialidades { get; set; } = null!;

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medico>()
            .HasKey(m => m.Id);

            //modelBuilder.Entity<Medico>()
            //    .HasMany(m => m.Horarios)
            //    .WithOne()
            //    .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Medico>()
                        .HasMany(m => m.Horarios)
                        .WithOne(h => h.Medico)
                        .HasForeignKey(h => h.MedicoId)
                        .OnDelete(DeleteBehavior.Cascade);

             modelBuilder.Entity<HorarioDisponivel>()
                .HasKey(h => h.Id);

            modelBuilder.Entity<Especialidade>()
                .HasKey(e => e.Id);


            modelBuilder.Entity<Medico>(ConfigureMedico);
            modelBuilder.Entity<HorarioDisponivel>(ConfigureHorarioDisponivel);
            modelBuilder.Entity<Especialidade>(ConfigureEspecialidade);

        }

        private void ConfigureMedico(EntityTypeBuilder<Medico> entity)
        {
            entity.ToTable("medicos");

            entity.HasKey(m => m.Id);
            entity.Property(m => m.Id).HasColumnName("id").HasColumnType("integer");
            entity.Property(m => m.Nome).HasColumnName("nome").HasMaxLength(255).IsRequired();
            entity.Property(m => m.CRM).HasColumnName("crm").HasMaxLength(50).IsRequired();
            entity.Property(m => m.Especialidade).HasColumnName("especialidade").HasMaxLength(100).IsRequired();
        }

        private void ConfigureEspecialidade(EntityTypeBuilder<Especialidade> entity)
        {
            entity.ToTable("especialidades"); // Nome da tabela

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasColumnType("integer");
            entity.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(255).IsRequired();
            entity.Property(e => e.Categoria).HasColumnName("categoria").HasMaxLength(100).IsRequired();
        }
        private void ConfigureHorarioDisponivel(EntityTypeBuilder<HorarioDisponivel> entity)
        {
            entity.ToTable("horarios_disponiveis");

            entity.HasKey(h => h.Id);

            entity.Property(h => h.Id)
                .HasColumnName("id")
                .HasColumnType("integer");

            entity.Property(h => h.MedicoId)
                .HasColumnName("medico_id")
                .IsRequired();

            entity.Property(c => c.DataHora).HasColumnName("data_hora").IsRequired().HasConversion(
             v => DateTime.SpecifyKind(v, DateTimeKind.Utc), // para o banco
             v => DateTime.SpecifyKind(v, DateTimeKind.Utc)); // do banco;
            
            entity.Property(h => h.ValorConsulta)
                .HasColumnName("valor_consulta")
                .HasColumnType("decimal(10,2)") 
                .IsRequired(); // se for NOT NULL

            entity.Property(h => h.Ocupado)
                .HasColumnName("ocupado")
                .HasColumnType("boolean")
                .HasDefaultValue(false);
        }
        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}
