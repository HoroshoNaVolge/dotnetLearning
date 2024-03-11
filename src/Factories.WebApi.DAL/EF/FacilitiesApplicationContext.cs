using Factories.WebApi.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Factories.WebApi.DAL.EF
{
    public class FacilitiesApplicationContext : DbContext
    {
        public DbSet<Factory> Factories { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Tank> Tanks { get; set; }
        public FacilitiesApplicationContext(DbContextOptions<FacilitiesApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}