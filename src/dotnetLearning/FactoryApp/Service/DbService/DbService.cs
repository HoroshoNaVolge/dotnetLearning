using dotnetLearning.FactoryApp.Model;
using dotnetLearning.FactoryApp.Service.FacilityService;
using Microsoft.EntityFrameworkCore;

namespace dotnetLearning.FactoryApp.Service.DbService
{
    public class DbFacilitiesService(DbContextOptions<ApplicationContext> options)
    {
        public async Task CreateAll(IList<Factory> factories, IList<Unit> units, IList<Tank> tanks, CancellationToken token)
        {
            await using ApplicationContext db = new(options);
            db.Factories.AddRange(factories);
            db.Units.AddRange(units);
            db.Tanks.AddRange(tanks);
            db.SaveChanges();
        }

        public async Task AddFacility(IFacility facility, CancellationToken token)
        {
            await using ApplicationContext db = new(options);
            if (facility is Factory factory)
                db.Factories.Add(factory);
            else if (facility is Unit unit)
                db.Units.Add(unit);
            else if (facility is Tank tank)
                db.Tanks.Add(tank);
            db.SaveChanges();
        }

        public async Task UpdateFacility(IFacility facility, CancellationToken token)
        {
            await using ApplicationContext db = new(options);
            if (facility is Factory factory)
                db.Factories.Update(factory);
            else if (facility is Unit unit)
                db.Units.Update(unit);
            else if (facility is Tank tank)
                db.Tanks.Update(tank);
            db.SaveChanges();
        }

        public async Task DeleteFacility(IFacility facility, CancellationToken token)
        {
            await using ApplicationContext db = new(options);
            if (facility is Factory factory)
                db.Factories.Remove(factory);
            else if (facility is Unit unit)
                db.Units.Remove(unit);
            else if (facility is Tank tank)
                db.Tanks.Remove(tank);
            db.SaveChanges();
        }

        public async Task GetFacilities(FacilitiesContainer container, CancellationToken token)
        {
            await using ApplicationContext db = new(options);
            container.Factories = [.. db.Factories];
            container.Units = [.. db.Units];
            container.Tanks = [.. db.Tanks];
        }
    }
}
