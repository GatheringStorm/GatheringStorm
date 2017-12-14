using GatheringStorm.Api.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace GatheringStorm.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MoveTargetEntity>()
                .HasKey(_ => new { _.EntityId, _.MoveId });
            modelBuilder.Entity<UserParticipation>()
                .HasKey(_ => new { _.UserId, _.GameId});
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<CardEffect> CardEffects { get; set; }
        public DbSet<CardLocation> CardLocations { get; set; }
        public DbSet<Effect> Effects { get; set; }
        public DbSet<Entity> Entities { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameCard> GameCards { get; set; }
        public DbSet<Move> Moves { get; set; }
        public DbSet<MoveTargetEntity> MoveTargetEntities { get; set; }
        public DbSet<MoveType> MoveTypes { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserParticipation> UserParitcipations { get; set; }
    }
}