using HealthMed.Patient.Application.Events;
using HealthMed.Patient.Application.Interfaces;
using HealthMed.Patient.Application.ViewModels;
using HealthMed.Patient.Domain.Entities;
using HealthMed.Patient.Domain.Interfaces;
using HealthMed.Patient.Domain.Validations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Patient.Application.UseCases
{
    public class DeletePacienteUseCase([FromServices]
    IPacientePublisher pacientePublisher /* Serviço dedicado para publicar */,
      IPacienteRepository pacienteRepository) : IDeletePacienteUseCase
    {
        public async Task<PublishResponse> Delete(int Id)
        {
            var contact = await pacienteRepository.ObterPorIdAsync(Id);

            if (contact is null)
                throw new ApplicationException("Paciente não encontrado");

            await pacientePublisher.PublishDeletePacienteAsync(new DeletePacienteEvent { Id = Id });

            return new PublishResponse
            {
                Message = "Exclusão em processamento.",
                Data = new
                {
                    contact.Id,
                    contact.Cpf,
                    contact.Email,

                }
            };
        }
    }
}
