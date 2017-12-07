using GatheringStorm.Api.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace GatheringStorm.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options): base(options)
        {
        }

        public DbSet<Entity> Entities { get; set; }
    }
}