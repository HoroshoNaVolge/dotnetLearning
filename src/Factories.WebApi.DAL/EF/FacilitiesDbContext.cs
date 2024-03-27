using Factories.WebApi.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Factories.WebApi.DAL.EF
{
    public class FacilitiesDbContext(DbContextOptions<FacilitiesDbContext> options) : DbContext(options)
    {
        public DbSet<Factory> Factories { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Tank> Tanks { get; set; }
    }
}