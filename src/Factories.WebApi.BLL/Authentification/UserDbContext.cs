using Factories.WebApi.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Factories.WebApi.BLL.Authentification
{
    public class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
