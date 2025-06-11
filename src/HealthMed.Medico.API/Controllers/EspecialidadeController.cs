using HealthMed.Doctor.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Doctor.API.Controllers
{
  
        [ApiController]
        [Route("api/[controller]")]
        [Authorize(Roles = "medico,paciente")]
        public class EspecialidadeController : ControllerBase
        {
            [HttpGet("listar-todas")]
            public async Task<IActionResult> ListarTodasAsync([FromServices] IGetEspecialidadeUseCase getEspecialidadeUseCase)
            {
                var result = await getEspecialidadeUseCase.ListarTodasAsync();
                return Ok(result);
            }

            [HttpGet("get-by-nome")]
            public async Task<IActionResult> GetByNomeAsync([FromServices] IGetEspecialidadeUseCase getEspecialidadeUseCase, [FromQuery] string nome)
            {
                var result = await getEspecialidadeUseCase.GetByNome(nome);
                return Ok(result);
            }

            [HttpGet("get-by-categoria")]
            public async Task<IActionResult> GetByCategoriaAsync([FromServices] IGetEspecialidadeUseCase getEspecialidadeUseCase, [FromQuery] string categoria)
            {
                var result = await getEspecialidadeUseCase.GetByCategoria(categoria);
                return Ok(result);
            }
        }
}
