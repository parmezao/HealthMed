using HealthMed.Patient.Application.Interfaces;
using HealthMed.Patient.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Patient.API.Controllers
{
    [ApiController]
    [Route("api/paciente")]
    public class PacienteController : ControllerBase
    {
        // TODO: Validar formato do CPF
        // TODO: Validar formato do Email
        // TODO: Validar Roles e Criação do Usuário no Cadastro
        [HttpPost("Cadastrar")]
        [AllowAnonymous]
        public async Task<IActionResult> CadastrarAsync(
            [FromServices] IInsertPacienteUseCase insertPacienteUseCase, 
            [FromBody] InsertPacienteRequest request) => Ok(await insertPacienteUseCase.Execute(request));

        [HttpPut("Atualizar")]
        [Authorize(Roles = "paciente")]
        public async Task<IActionResult> AtualizarAsync(
            [FromServices] IUpdatePacienteUseCase updatePacienteUseCase ,  
            [FromBody] UpdatePacienteRequest request) => Ok(await updatePacienteUseCase.Execute( request));

        //TODO: Excluir cadastro

        [HttpGet("ObterPorCpf")]
        [Authorize(Roles = "paciente, medico")]
        public async Task<IActionResult> ObterPorCpfAsync(
            [FromServices] IGetPacienteUseCase getPacienteUseCase, string cpf) => Ok(await getPacienteUseCase.ObterPorCpfAsync(cpf));

        [HttpGet("ObterPorId")]
        [Authorize(Roles = "paciente, medico")]
        public async Task<IActionResult> ObterPorIdAsync([FromServices] IGetPacienteUseCase getPacienteUseCase,int id)
            => Ok(await getPacienteUseCase.ObterPorIdAsync(id));

        [HttpGet("ObterTodos")]
        [Authorize(Roles = "paciente, medico")]
        public async Task<IActionResult> ObterTodosAsync([FromServices] IGetPacienteUseCase getPacienteUseCase)
            => Ok(await getPacienteUseCase.ObterTodosAsync());
    }
}
