using System.Threading;
using System.Threading.Tasks;
using GatheringStorm.Api.Models;
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

            modelBuilder.Entity<User>()
                .HasMany(_ => _.Participations)
                .WithOne(_ => _.User)
                .HasForeignKey(_ => _.Mail);

            modelBuilder.Entity<UserParticipation>()
                .HasKey(_ => new { _.Mail, _.GameId });

            modelBuilder.Entity<ClassChoice>()
                .HasKey(_ => new { _.ClassType, _.GameId, _.Mail });

            modelBuilder.Entity<UserParticipation>()
                .HasMany(_ => _.ClassChoices)
                .WithOne(_ => _.UserParticipation)
                .HasForeignKey(_ => new { _.Mail, _.GameId });
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<CardEffect> CardEffects { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<ClassChoice> ClassChoices { get; set; }
        public DbSet<Entity> Entities { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameCard> GameCards { get; set; }
        public DbSet<Move> Moves { get; set; }
        public DbSet<MoveTargetEntity> MoveTargetEntities { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserParticipation> UserParitcipations { get; set; }
    }

    public static class DbContextExtensions
    {
        public static async Task<AppResult<T>> FindEntity<T>(this DbSet<T> dbSet, object id, CancellationToken cancellationToken = default(CancellationToken))
            where T : class
        {
            var result = await dbSet.FindAsync(new object[] { id }, cancellationToken);
            if (result != null)
            {
                return AppResult<T>.Success(result);
            }
            else
            {
                return VoidAppResult.Error(ErrorPreset.OnLoadingData).GetErrorAppResult<T>();
            }
        }
    }
}