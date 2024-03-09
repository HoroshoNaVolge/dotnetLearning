using Factories.WebApi.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Factories.WebApi.DAL.EF
{
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