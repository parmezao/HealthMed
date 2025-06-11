using HealthMed.Consultation.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Consultation.Application.Interfaces
{
  public  interface IAgendarConsultaUseCase
    {
        Task<PublishResponse> Execute(AgendarConsultaRequest agendarConsultaRequest);
    }
}
