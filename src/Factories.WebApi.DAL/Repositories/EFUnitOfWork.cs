using Factories.WebApi.DAL.EF;
using Factories.WebApi.DAL.Entities;
using Factories.WebApi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Factories.WebApi.DAL.Repositories
{
    public class EFUnitOfWork(DbContextOptions<FacilitiesApplicationContext> options) : IUnitOfWork
    {
        private readonly FacilitiesApplicationContext db = new FacilitiesApplicationContext(options) ?? throw new ArgumentNullException(nameof(options));
        private readonly FactoryRepository? factoriesRepository;
        private readonly UnitRepository? unitsRepository;
        private readonly TankRepository? tanksRepository;

        private readonly object lockObject = new();
        public IRepository<Factory> Factories
        {
            get
            {
                lock (lockObject)
                {
                    if (factoriesRepository != null)
                        return factoriesRepository;

                    return new FactoryRepository(db);
                }
            }
        }

        public IRepository<Unit> Units
        {
            get
            {
                lock (lockObject)
                {
                    if (unitsRepository != null)
                        return unitsRepository;
                    return new UnitRepository(db);
                }
            }
        }

        public IRepository<Tank> Tanks
        {
            get
            {
                lock (lockObject)
                {
                    if (tanksRepository != null)
                        return tanksRepository;
                    return new TankRepository(db);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async void Save() => await db.SaveChangesAsync();

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    db.Dispose();

                disposed = true;
            }
        }
    }
}