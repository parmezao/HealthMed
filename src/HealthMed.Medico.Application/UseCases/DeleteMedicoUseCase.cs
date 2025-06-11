using HealthMed.Doctor.Application.Events;
using HealthMed.Doctor.Application.Interfaces;
using HealthMed.Doctor.Application.ViewModels;
using HealthMed.Doctor.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Application.UseCases
{
    public class DeleteMedicoUseCase([FromServices] IMedicoPublisher contactPublisher /* Serviço dedicado para publicar */,
    IMedicoRepository medicoRepository) : IDeleteMedicoUseCase
    {
        public async Task<PublishResponse> Delete(int Id)
        {
            var medico = await medicoRepository.ObterPorIdAsync(Id);

            if (medico is null)
                throw new ApplicationException("Médico não encontrado");

            await contactPublisher.PublishDeleteMedicotAsync(new DeleteMedicoEvent { Id = Id });

            return new PublishResponse
            {
                Message = "Exclusão em processamento.",
                Data = new
                {
                    medico.Id,
                    medico.Nome,
                    medico.Especialidade 
                }
            };
        }
    }
}
