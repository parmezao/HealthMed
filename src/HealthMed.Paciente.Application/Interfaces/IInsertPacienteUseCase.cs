using HealthMed.Patient.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Patient.Application.Interfaces
{
  public  interface IInsertPacienteUseCase
    {
        Task<PublishResponse> Execute(InsertPacienteRequest insertPacienteRequest);
    }
}
