using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Consultation.Domain.Core
{
    public interface IRepository : IDisposable
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
