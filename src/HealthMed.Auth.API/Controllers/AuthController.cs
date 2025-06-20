using HealthMed.Auth.Application.Interfaces;
using HealthMed.Auth.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Auth.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(ILoginUseCase loginUseCase, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly ILoginUseCase _loginUseCase = loginUseCase;
 
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Senha))
                return BadRequest("Login e senha são obrigatórios.");

            try
            {
                var response = await _loginUseCase.ExecuteAsync(request);

                if (response is null)
                    return Unauthorized("Credenciais inválidas.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao autenticar o usuário. {ex.Message}");
                return StatusCode(500, "Erro interno ao processar login.");
            }
        }
    }
}
