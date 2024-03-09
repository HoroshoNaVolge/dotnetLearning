using Factories.WebApi.DAL.EF;
using Factories.WebApi.DAL.Entities;
using Factories.WebApi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Factories.WebApi.DAL.Repositories
{
    public class UnitRepository(ApplicationContext db) : IRepository<Unit>
    {
        private readonly ApplicationContext db = db;

        public void Create(Unit item)
        {
            db.Units.Add(item);
        }

        public void Delete(int id)
        {
            Unit? item = db.Units.Find(id);
            if (item != null)
                db.Units.Remove(item);
        }

        public IEnumerable<Unit> Find(Func<Unit, bool> predicate)
        {
            return db.Units.Where(predicate).ToList();
        }

        public Unit? Get(int id)
        {
            return db.Units.Find(id);
        }

        public IEnumerable<Unit>? GetAll()
        {
            return db.Units;
        }

        public void Update(Unit item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}