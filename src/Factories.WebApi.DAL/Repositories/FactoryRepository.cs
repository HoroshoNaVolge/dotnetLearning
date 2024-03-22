using Factories.WebApi.DAL.EF;
using Factories.WebApi.DAL.Entities;
using Factories.WebApi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Factories.WebApi.DAL.Repositories
{
    public class FactoryRepository(FacilitiesApplicationContext db) : IRepository<Factory>
    {
        private readonly FacilitiesApplicationContext db = db;

        public void Create(Factory item) => db.Factories.Add(item);

        public void Delete(int id)
        {
            Factory? item = db.Factories.Find(id);
            if (item != null)
                db.Factories.Remove(item);
        }

        public IEnumerable<Factory> Find(Func<Factory, bool> predicate) => db.Factories.Where(predicate).ToList();

        public Factory? Get(int id) => db.Factories.Find(id);

        public async Task<IEnumerable<Factory>>? GetAllAsync(CancellationToken token) => 
            await db.Factories.ToListAsync(token);

        public void Update(int id, Factory factoryToUpdate)
        {
            var existingFactory = db.Factories.Find(id) ?? throw new InvalidOperationException("Factory not found");

            db.Entry(existingFactory).CurrentValues.SetValues(factoryToUpdate);
        }
    }
}
