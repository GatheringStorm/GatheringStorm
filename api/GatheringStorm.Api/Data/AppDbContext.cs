using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GatheringStorm.Api.Models;
using GatheringStorm.Api.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace GatheringStorm.Api.Data
{
    public class AppDbContext : DbContext
    {
        public Guid Id { get; } = Guid.NewGuid();

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

    public static class AppDbContextExtensions
    {
        public static IQueryable<Game> IncludeUserParticipations(this IQueryable<Game> dbSet)
        {
            return dbSet
                .Include(_ => _.UserParticipations)
                    .ThenInclude(_ => _.User)
                .Include(_ => _.UserParticipations)
                    .ThenInclude(_ => _.ClassChoices);
        }

        public static IQueryable<Game> IncludeEntities(this IQueryable<Game> dbSet)
        {
            return dbSet
                .Include(_ => _.Entities)
                    .ThenInclude(_ => _.User);
        }

        public static IQueryable<GameCard> IncludeAll(this IQueryable<GameCard> dbSet)
        {
            return dbSet
                .IncludeCards()
                .Include(_ => _.User)
                .Include(_ => _.Game);
        }

        public static IQueryable<GameCard> IncludeCards(this IQueryable<GameCard> dbSet)
        {
            return dbSet
                .Include(_ => _.Card)
                    .ThenInclude(_ => _.Character)
                .Include(_ => _.Card)
                    .ThenInclude(_ => _.Title)
                .Include(_ => _.Card)
                    .ThenInclude(_ => _.Effects);
        }
    }
}