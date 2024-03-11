using Factories.WebApi.DAL.EF;
using Factories.WebApi.DAL.Entities;
using Factories.WebApi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Factories.WebApi.DAL.Repositories
{
    public class UnitRepository(FacilitiesApplicationContext db) : IRepository<Unit>
    {
        private readonly FacilitiesApplicationContext db = db;

        public void Create(Unit item) => db.Units.Add(item);

        public void Delete(int id)
        {
            Unit? item = db.Units.Find(id);
            if (item != null)
                db.Units.Remove(item);
        }

        public IEnumerable<Unit> Find(Func<Unit, bool> predicate) => db.Units.Where(predicate).ToList();

        public Unit? Get(int id) => db.Units.Find(id);

        public async Task<IEnumerable<Unit>> GetAllAsync(CancellationToken token) => await db.Units.ToListAsync(token == default ? CancellationToken.None : token);

        public void Update(Unit item) => db.Entry(item).State = EntityState.Modified;
    }
}