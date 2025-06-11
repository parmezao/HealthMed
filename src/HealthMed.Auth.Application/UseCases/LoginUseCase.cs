using HealthMed.Auth.Application.Interfaces;
using HealthMed.Auth.Application.ViewModels;
using HealthMed.Auth.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthMed.Auth.Application.UseCases
{
    public class LoginUseCase(IConfiguration configuration, IUsuarioRepository repository) : ILoginUseCase
    {
        public async Task<LoginResponse?> ExecuteAsync(LoginRequest request)
        {
            // 1. Validação de entrada
            if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Senha))
                return null;

            // 2. Buscar usuário
            var usuario = await repository.ObterPorLoginAsync(request.Login);
            if (usuario == null)
                return null;

            // 3. Verificar senha
            var senhaValida = await repository.VerificarSenhaAsync(usuario, request.Senha);
            if (!senhaValida)
                return null;

            // 4. Validar secret JWT
            var secret = configuration["SecretJWT"];
            if (string.IsNullOrWhiteSpace(secret))
                throw new InvalidOperationException("SecretJWT não configurado.");

            var chaveCriptografia = Encoding.ASCII.GetBytes(secret);

            // 5. Claims personalizadas
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Role, usuario.Role)
            };

            if (!string.IsNullOrWhiteSpace(usuario.Cpf))
                claims.Add(new Claim("cpf", usuario.Cpf));

            if (!string.IsNullOrWhiteSpace(usuario.Crm))
                claims.Add(new Claim("crm", usuario.Crm));

            // 6. Criar token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(4),
                Issuer = "HealthMed.Auth.API",
                Audience = "healthmed-api",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(chaveCriptografia),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponse
            {
                Token = tokenHandler.WriteToken(token),
                Role = usuario.Role,
                Nome = usuario.Nome
            };
        }
    }
}
