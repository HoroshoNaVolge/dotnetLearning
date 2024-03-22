using Factories.WebApi.DAL.Entities;
using Factories.WebApi.DAL.UsersAndRoles;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Factories.WebApi.DAL.EF
{
    public class FacilitiesApplicationContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>

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