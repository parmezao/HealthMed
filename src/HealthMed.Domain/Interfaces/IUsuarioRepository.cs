using HealthMed.Auth.Domain.Core;
using HealthMed.Auth.Domain.Entities;

namespace HealthMed.Auth.Domain.Interfaces
{
    public interface IUsuarioRepository: IRepository
    {
        Task<Usuario> ObterPorLoginAsync(string login);
        Task<bool> VerificarSenhaAsync(Usuario usuario, string senha);
    }
}
