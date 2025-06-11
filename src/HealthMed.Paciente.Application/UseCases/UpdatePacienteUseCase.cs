using HealthMed.Patient.Application.Events;
using HealthMed.Patient.Application.Interfaces;
using HealthMed.Patient.Application.ViewModels;
using HealthMed.Patient.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Patient.Application.UseCases
{
    public class UpdatePacienteUseCase([FromServices] IPacientePublisher pacientetPublisher /* Serviço dedicado para publicar */,
        IPacienteRepository pacienteRepository) : IUpdatePacienteUseCase 
    {
        public async Task<PublishResponse> Execute(UpdatePacienteRequest updatePacienteRequest)
        {
            var contact = await pacienteRepository.ObterPorIdAsync(updatePacienteRequest.Id);
            if (contact == null) throw new ApplicationException("Não foi possível localizar o cadastro do paciente informado.");

            // TODO: Validar se o CPF poderá ser alterado, em caso positivo, validar se já não está vinculado a outro paciente

            // TODO: Validar se o E-mail já não está vinculado a outro paciente

            await pacientetPublisher.PublishUpdatePacienteAsync(new UpdatePacienteEvent
            {
                Id = updatePacienteRequest.Id,
                Nome = updatePacienteRequest.Nome,
                Email = updatePacienteRequest.Email,
                Cpf   = updatePacienteRequest.Cpf
            });

            return new PublishResponse
            {
                Message = "Atualização em processamento.",
                Data = new
                {
                    updatePacienteRequest.Id,
                    updatePacienteRequest.Nome,
                    updatePacienteRequest.Email,
                    updatePacienteRequest.Cpf
                }
            };
        }
    }
}

 
