
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Gateway.API.Controllers
{
    [ApiController]
    [Route("api/swagger-links")]
    public class SwaggerLinksController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetSwaggerLinks()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var swaggerLinks = new[]
            {
                new SwaggerLink("Auth.API",     $"{baseUrl}/auth/swagger/index.html"),
                new SwaggerLink("Medico.API",   $"{baseUrl}/medico/swagger/index.html"),
                new SwaggerLink("Paciente.API", $"{baseUrl}/paciente/swagger/index.html"),
                new SwaggerLink("Agenda.API",   $"{baseUrl}/agenda/swagger/index.html"),
                new SwaggerLink("Consulta.API", $"{baseUrl}/consulta/swagger/index.html")
            };

            return Ok(swaggerLinks);
        }

        public record SwaggerLink(string Servico, string Url);
    }
}
