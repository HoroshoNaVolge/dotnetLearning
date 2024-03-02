using dotnetLearning.FactoryApp.Model;
using dotnetLearning.FactoryApp.Service.FacilityService;
using Microsoft.EntityFrameworkCore;

namespace dotnetLearning.FactoryApp.Service.SerializationService
{
    public class DbFacilitiesService(DbContextOptions<ApplicationContext> options) : ISerializationService
    {
        public async Task CreateOrUpdateAllAsync(FacilitiesContainer container, CancellationToken token)
        {
            await using ApplicationContext db = new(options);
            db.Factories.AddRange(container.Factories);
            db.Units.AddRange(container.Units);
            db.Tanks.AddRange(container.Tanks);
            db.SaveChanges();
        }

        public async Task AddFacilityAsync(IFacility facility, CancellationToken token)
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

        public async Task UpdateFacilityAsync(IFacility facility, CancellationToken token)
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

        public async Task DeleteFacilityAsync(IFacility facility, CancellationToken token)
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

        public async Task GetFacilitiesAsync(FacilitiesContainer container, CancellationToken token)
        {
            await using ApplicationContext db = new(options);
            container.Factories = [.. db.Factories];
            container.Units = [.. db.Units];
            container.Tanks = [.. db.Tanks];
        }
    }

    public class ApplicationContext : DbContext
    {
        public DbSet<Factory> Factories { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Tank> Tanks { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
