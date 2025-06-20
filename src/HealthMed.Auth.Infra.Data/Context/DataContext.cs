using HealthMed.Auth.Domain.Core;
using HealthMed.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.Auth.Infra.Data.Context
{
    public class DataContext : DbContext, IUnitOfWork
    {
        public DbSet<Usuario> Usuarios => Set<Usuario>();

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(ConfigureUsuario);
        }

        private void ConfigureUsuario(EntityTypeBuilder<Usuario> entity)
        {
            entity.ToTable("usuarios")
                .HasDiscriminator<string>("role")
                .HasValue<Medico>("medico")
                .HasValue<Paciente>("paciente");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Id)
                .HasColumnName("id")
                .HasColumnType("integer");

            entity.Property(p => p.Nome)
                .HasColumnName("nome")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(p => p.SenhaHash)
                .HasColumnName("senha_hash")
                .HasMaxLength(1000)
                .IsRequired();

            entity.Property(p => p.Cpf)
                .HasColumnName("cpf")
                .HasMaxLength(11);

            entity.Property(p => p.Email)
                .HasColumnName("email")
                .HasMaxLength(255);

            entity.Property(p => p.Crm)
                .HasColumnName("crm")
                .HasMaxLength(50);

            // ⚠️ NÃO mapear Role manualmente
            // ⚠️ NÃO usar entity.Ignore(p => p.Role);
        }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}
