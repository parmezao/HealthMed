using HealthMed.Patient.Domain.Core;
using HealthMed.Patient.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Patient.Infra.Data.Context
{
    public class DataContext : DbContext, IUnitOfWork
    {
        public DbSet<Paciente> Pacientes => Set<Paciente>();

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Paciente>(ConfigurePaciente);
        }
        private void ConfigurePaciente(EntityTypeBuilder<Paciente> entity)
        {
            entity.ToTable("pacientes");

            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("id").HasColumnType("integer");
            entity.Property(p => p.Nome).HasColumnName("nome").HasMaxLength(255).IsRequired();
            entity.Property(p => p.Cpf).HasColumnName("cpf").HasMaxLength(11).IsRequired();
            entity.Property(p => p.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
        }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}
