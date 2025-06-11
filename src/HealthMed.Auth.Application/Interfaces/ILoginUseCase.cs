using HealthMed.Auth.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Auth.Application.Interfaces
{
  public   interface ILoginUseCase
    {
        Task<LoginResponse> ExecuteAsync(LoginRequest request);
    }
}
