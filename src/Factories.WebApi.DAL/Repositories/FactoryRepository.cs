using Factories.WebApi.DAL.EF;
using Factories.WebApi.DAL.Entities;
using Factories.WebApi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Factories.WebApi.DAL.Repositories
{
    public class FactoryRepository(ApplicationContext db) : IRepository<Factory>
    {
        private readonly ApplicationContext db = db;

        public void Create(Factory item)
        {
            db.Factories.Add(item);
        }

        public void Delete(int id)
        {
            Factory? item = db.Factories.Find(id);
            if (item != null)
                db.Factories.Remove(item);
        }

        public IEnumerable<Factory> Find(Func<Factory, bool> predicate)
        {
            return db.Factories.Where(predicate).ToList();
        }

        public Factory? Get(int id)
        {
            return db.Factories.Find(id);
        }

        public IEnumerable<Factory>? GetAll()
        {
            return db.Factories;
        }

        public void Update(Factory item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
