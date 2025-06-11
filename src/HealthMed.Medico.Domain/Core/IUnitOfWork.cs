using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Doctor.Domain.Core
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();


    }
}
