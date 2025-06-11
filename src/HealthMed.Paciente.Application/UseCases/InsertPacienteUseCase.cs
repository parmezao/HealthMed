using HealthMed.Patient.Application.Events;
using HealthMed.Patient.Application.Interfaces;
using HealthMed.Patient.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Patient.Application.UseCases
{
    public class InsertPacienteUseCase([FromServices] IPacientePublisher pacientePublisher /* Serviço dedicado para publicar */) : IInsertPacienteUseCase
    {
        public async Task<PublishResponse> Execute(InsertPacienteRequest insertPacienteRequest)
        {
            // Cria a mensagem e publica na fila
            await pacientePublisher.PublishInsertPacienteAsync(new InsertPacienteEvent
            {
                Nome = insertPacienteRequest.Nome,
                Email = insertPacienteRequest.Email,
                Cpf = insertPacienteRequest.Cpf
            });

            return new PublishResponse
            {
                Message = "Cadastro em processamento.",
                Data = new
                {
                    insertPacienteRequest.Nome,
                    insertPacienteRequest.Email,
                    insertPacienteRequest.Cpf
                }
            };
        }
    }   
}
