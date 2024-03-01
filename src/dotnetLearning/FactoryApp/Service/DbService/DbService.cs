using dotnetLearning.FactoryApp.Model;
using Microsoft.EntityFrameworkCore;

namespace dotnetLearning.FactoryApp.Service.DbService
{
    public class DbFacilitiesService(DbContextOptions<ApplicationContext> options)
    {
        public async Task CreateAll(IList<Factory> factories, IList<Unit> units, IList<Tank> tanks, CancellationToken token)
        {
            await using ApplicationContext db = new ApplicationContext(options);
            foreach (var fact in factories) { db.Factories.Add(fact); }
            foreach (var unit in units) { db.Units.Add(unit); }
            foreach (var tank in tanks) { db.Tanks.Add(tank); }
            db.SaveChanges();
        }
    }
}
