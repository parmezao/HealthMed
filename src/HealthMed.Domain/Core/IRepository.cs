namespace HealthMed.Auth.Domain.Core
{
    public interface IRepository : IDisposable
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
