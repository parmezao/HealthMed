using HealthMed.Auth.Domain.Core;
using HealthMed.Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Auth.Domain.Interfaces
{
   public  interface IUsuarioRepository: IRepository
    {
        Task<Usuario> ObterPorLoginAsync(string login);
        Task<bool> VerificarSenhaAsync(Usuario usuario, string senha);
    }
}
