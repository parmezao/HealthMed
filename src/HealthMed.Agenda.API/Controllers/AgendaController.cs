using HealthMed.Agenda.Application.Interfaces;
using HealthMed.Agenda.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Agenda.API.Controllers
{
    [ApiController]
    [Route("api/agenda")]
    public class AgendaController : ControllerBase
    {
        private readonly IAgendaUseCase _useCase;

        public AgendaController(IAgendaUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpPost("horarios")]
        [Authorize(Roles = "medico")]
        public async Task<IActionResult> Cadastrar([FromServices] ICadastrarHorarioUseCase useCase, [FromBody] CadastrarHorarioRequest request)
        {
            return Ok(await useCase.ExecuteAsync(request));
        }

        [HttpPatch("horarios")]
        [Authorize(Roles = "medico")]
        public async Task<IActionResult> Editar([FromServices] IEditarHorarioUseCase useCase, [FromBody] EditarHorarioRequest request)
        {
            return Ok(await useCase.ExecuteAsync(request));
        }

        // TODO: Exclusão lógica
        [HttpDelete("horarios")]
        [Authorize(Roles = "medico")]
        public async Task<IActionResult> Excluir([FromServices] IRemoverHorarioUseCase useCase, [FromBody] RemoverHorarioRequest request)
        {
            return Ok(await useCase.ExecuteAsync(request));
        }

        // TODO: Separar casos de uso
        [HttpGet("horarios/{id}")]
        [Authorize(Roles = "medico")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var result = await _useCase.ObterPorIdAsync(id);
            return Ok(result);
        }

        //TODO: Obter além dos horários disponíveis, o valor da consulta
        [HttpGet("medico/{medicoId}")]
        [AllowAnonymous]
        public async Task<IActionResult> ObterPorMedico(int medicoId)
        {
            var result = await _useCase.ObterPorMedicoAsync(medicoId);
            return Ok(result);
        }

        [HttpPatch("ocupar/{horarioId}")]
        [Authorize(Roles = "sistema")]
        public async Task<IActionResult> MarcarComoOcupado(int horarioId)
        {
            await _useCase.MarcarComoOcupadoAsync(horarioId);
            return Ok("Horário marcado como ocupado.");
        }
    }
}
 
