using HealthMed.Doctor.Application.Interfaces;
using HealthMed.Doctor.Application.UseCases;
using HealthMed.Doctor.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Doctor.API.Controllers
{
    [Route("api/[controller]")]
    public class MedicoController : ControllerBase
    {
        /// <summary>
        /// Inclusão de um Médico
        /// </summary>
        /// <param name="insertMedicoUseCase">Médico a ser incluído</param>
        /// <param name="insertMedicoRequest">Médico a ser incluído</param>
        /// <returns>Retorna o Médico incluído</returns>
        /// <response code="200">Sucesso na inclusão do Médico</response>
        /// <response code="400">Não foi possível incluir o Médico</response>
        /// <response code="401">Não autorizado</response>
        [HttpPost]
        [Route("Insert")]
        [Authorize(Roles = "medico")]
        public async Task<IActionResult> Add([FromServices] IInsertMedicoUseCase insertMedicoUseCase, [FromBody] InsertMedicoRequest insertMedicoRequest)
        {
            try
            {
                return Ok(await insertMedicoUseCase.Execute(insertMedicoRequest));
            }
            catch (Exception e) when (e is ApplicationException || e is ArgumentException)
            {
                return BadRequest(new ErrorMessageResponse(e.Message));
            }
        }
        /// <summary>
        /// Alteração de um Médico
        /// </summary>
        /// <param name="updateMedicoUseCase">Médico a ser alterado</param>
        /// <param name="updateMedicoRequest">Médico a ser alterado</param>
        /// <returns>Retorna o Médico alterado</returns>
        /// <response code="200">Sucesso na alteração do Médico</response>
        /// <response code="400">Não foi possível alterar o Médico</response>
        /// <response code="401">Não autorizado</response>
        [HttpPut]
        [Route("Update")]
        [Authorize(Roles = "medico")]
        public async Task<IActionResult> Update([FromServices] IUpdateMedicoUseCase updateMedicoUseCase, [FromBody] UpdateMedicoRequest updateMedicoRequest)
        {
            try
            {
                return Ok(await updateMedicoUseCase.Execute(updateMedicoRequest));

            }
            catch (Exception e) when (e is ApplicationException || e is ArgumentException)
            {
                return BadRequest(new ErrorMessageResponse(e.Message));
            }
        }

        /// <summary>
        /// Exclusão de um Médico
        /// </summary>
        /// <param name="deleteMedicosUseCase">Exclusão de um Médico</param>
        /// <param name="Id">Identificador do Médico</param>
        /// <returns></returns>
        /// <response code="200">Sucesso na exclusão do Médico</response>
        /// <response code="400">Não foi possível excluir o Médico</response>
        /// <response code="401">Não autorizado</response>
        [HttpDelete]
        [Route("Delete")]
        [Authorize(Roles = "medico")]
        public async Task<IActionResult> Delete([FromServices] IDeleteMedicoUseCase deleteMedicosUseCase, [FromQuery] int Id)
        {
            try
            {
                return Ok(await deleteMedicosUseCase.Delete(Id));
            }
            catch (ApplicationException e)
            {
                return BadRequest(new ErrorMessageResponse(e.Message));
            }
        }

        /// <summary>
        /// Retorna todos os Médicos incluídos
        /// </summary>
        /// <param name="getMedicosUseCase"></param>
        /// <returns>Retorna a </returns>
        /// <response code="200">Sucesso na execução do retorno dos Médicos</response>
        /// <response code="400">Não foi possível retornar os Médicos</response>
        /// <response code="401">Não autorizado</response>
        [HttpGet("GetAll")]
        [Authorize(Roles = "medico, paciente")]
        public async Task<IActionResult> GetAll([FromServices] IGetMedicosUseCase getMedicosUseCase, [FromQuery]string? especialidade)
        {
            return Ok(await getMedicosUseCase.GetAll(especialidade));
        }


        /// <summary>
        /// Retorna os Médicos incluídos
        /// </summary>
        /// <param name="GetMedicosUseCase">Retorna os Médicos incluídos</param>
        /// <param name="crm">Informe o crm do Médico</param>
        /// <returns>Retorna a lista de Médicos incluídos</returns>
        /// <response code="200">Sucesso na execução do retorno dos Médicos</response>
        /// <response code="400">Não foi possível retornar os Médicos</response>
        /// <response code="401">Não autorizado</response>
        [HttpGet("GetByCRM")]
        [Authorize(Roles = "medico, paciente")]
        public async Task<IActionResult> Get([FromServices] IGetMedicosUseCase getMedicosUseCase, [FromQuery] string? crm)
        {
            return Ok(await getMedicosUseCase.ObterPorCrmAsync(crm));
        }

        /// <summary>
        /// Retorna o  Médico  pelo Id
        /// </summary>
        /// <param name="GetMedicosUseCase"></param>
        /// <param name="Id">Informe o id do Médico (Id)</param>
        /// <returns>Retorna a </returns>
        /// <response code="200">Sucesso na execução do retorno do  Médico </response>
        /// <response code="400">Não foi possível retornar o  Médico </response>
        /// <response code="401">Não autorizado</response>
        [HttpGet("GetById")]
        [Authorize(Roles = "medico, paciente")]
        public async Task<IActionResult> GetByIdAsync([FromServices] IGetMedicosUseCase getContactsUseCase, [FromQuery] int Id)
        {
            return Ok(await getContactsUseCase.ObterPorIdAsync(Id));
        }

        // TODO: Buscar por especialidade
    }
}
