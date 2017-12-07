using GatheringStorm.Api.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace GatheringStorm.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Entity> Entities { get; set; }
        public DbSet<User> Users { get; set; }
    }
}