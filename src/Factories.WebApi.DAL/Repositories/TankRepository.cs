using Factories.WebApi.DAL.EF;
using Factories.WebApi.DAL.Entities;
using Factories.WebApi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Tanks.WebApi.DAL.Repositories
{
    public class TankRepository(FacilitiesApplicationContext db) : IRepository<Tank>
    {
        private readonly FacilitiesApplicationContext db = db;

        public void Create(Tank item) => db.Tanks.Add(item);

        public void Delete(int id)
        {
            Tank? item = db.Tanks.Find(id);
            if (item != null)
                db.Tanks.Remove(item);
        }

        public IEnumerable<Tank> Find(Func<Tank, bool> predicate) => db.Tanks.Where(predicate).ToList();

        public Tank? Get(int id) => db.Tanks.Find(id);

        public async Task<IEnumerable<Tank>>? GetAllAsync(CancellationToken token) => await db.Tanks.ToListAsync(token == default ? CancellationToken.None : token);

        public void Update(Tank item) => db.Entry(item).State = EntityState.Modified;
    }
}