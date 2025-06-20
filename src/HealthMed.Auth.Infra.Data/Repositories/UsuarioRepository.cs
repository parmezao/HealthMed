using HealthMed.Auth.Domain.Core;
using HealthMed.Auth.Domain.Entities;
using HealthMed.Auth.Domain.Interfaces;
using HealthMed.Auth.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Auth.Infra.Data.Repositories
{
    public class UsuarioRepository(DataContext dataContext) : IUsuarioRepository
    {
        public IUnitOfWork UnitOfWork => dataContext;

        public void Dispose()
        {
            dataContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<Usuario> ObterPorLoginAsync(string login)
        {
            // Busca como médico usando OfType
            var medico = await dataContext.Usuarios
                .OfType<Medico>()
                .FirstOrDefaultAsync(m => m.Crm == login);

            if (medico is not null)
                return medico;

            // Busca como paciente usando OfType
            return await dataContext.Usuarios
                .OfType<Paciente>()
                .FirstOrDefaultAsync(p => p.Email == login || p.Cpf == login);
        }


        public Task<bool> VerificarSenhaAsync(Usuario usuario, string senha)
        {
            ////string hash = BCrypt.Net.BCrypt.HashPassword("123456", workFactor: 11);
            ////bool valido = BCrypt.Net.BCrypt.Verify("123456", hash);            

            return Task.FromResult(BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash));
        }
    }
}
