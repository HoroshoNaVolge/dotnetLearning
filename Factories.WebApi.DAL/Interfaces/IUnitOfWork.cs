using Factories.WebApi.DAL.Entities;

namespace Factories.WebApi.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Factory> Factories { get; }
        IRepository<Unit> Units { get; }
        IRepository<Tank> Tanks { get; }
        new void Dispose();
        void Save();
    }
}
