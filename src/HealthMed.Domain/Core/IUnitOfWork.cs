using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Auth.Domain.Core
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
