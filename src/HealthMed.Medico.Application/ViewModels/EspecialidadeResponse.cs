using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Application.ViewModels
{
    public record EspecialidadeResponse(
                  int Id,
                  string Nome,
                  string Categoria
        );
    
}
