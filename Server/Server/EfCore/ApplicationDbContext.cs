using Microsoft.EntityFrameworkCore;
using Server.EfCore.Model;

namespace Server.Model
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=emies;Username=postgres;Password=1123");
        }
    }
}
