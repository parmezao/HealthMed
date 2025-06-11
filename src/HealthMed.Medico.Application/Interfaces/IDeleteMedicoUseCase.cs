using HealthMed.Doctor.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Application.Interfaces
{
 public   interface IDeleteMedicoUseCase
    {
        Task<PublishResponse> Delete(int Id);
    }
}
