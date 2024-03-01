using dotnetLearning.FactoryApp.Model;
using Microsoft.EntityFrameworkCore;

namespace dotnetLearning.FactoryApp.Service.DbService
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
