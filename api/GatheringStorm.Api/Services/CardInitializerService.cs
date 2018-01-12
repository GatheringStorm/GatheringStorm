using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GatheringStorm.Api.Data;
using GatheringStorm.Api.Models;
using GatheringStorm.Api.Models.DB;
using GatheringStorm.Api.Models.DB.Effects;
using Newtonsoft.Json;

namespace GatheringStorm.Api.Services
{
    public interface ICardInitializerService
    {
        Task<VoidAppResult> InitializeCards();
    }

    public class CardInitializerService : ICardInitializerService
    {
        private readonly AppDbContext dbContext;

        public CardInitializerService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        public async Task<VoidAppResult> InitializeCards()
        {
            if (this.dbContext.Cards.Any())
            {
                return VoidAppResult.Success();
            }

            var claus = new Character
            {
                Id = Guid.NewGuid(),
                Name = "Claus"
            };
            var sepp = new Character
            {
                Id = Guid.NewGuid(),
                Name = "Sepp"
            };
            await dbContext.Characters.AddRangeAsync(claus, sepp);

            var pawn = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The pawn"
            };
            var bard = new Title
            {
                Id = Guid.NewGuid(),
                Name = "The bard"
            };
            await dbContext.Titles.AddRangeAsync(pawn);

            await dbContext.Cards.AddRangeAsync(new Card
            {
                Id = Guid.Parse("00000000-0000-4000-0000-000000000001"),
                Cost = 1,
                Attack = 2,
                BaseHealth = 1,
                Title = pawn,
                Character = claus
            },
            new Card
            {
                Id = Guid.Parse("00000000-0000-4000-0000-000000000002"),
                Cost = 1,
                Attack = 2,
                BaseHealth = 1,
                Title = pawn,
                Character = claus
            },
            new Card
            {
                Id = Guid.Parse("00000000-0000-4000-0000-000000000019"),
                Cost = 5,
                Attack = 4,
                BaseHealth = 4,
                Title = bard,
                Character = sepp,
                Effects = new List<CardEffect>
                {
                    new CardEffect
                    {
                        Id = Guid.NewGuid(),
                        EffectType = EffectType.ChangeStats,
                        EffectParameters = JsonConvert.SerializeObject(new ChangeStatsEffectParameters
                        {
                            TargetingType = TargetingType.NumberOfTargets,
                            TargetParameter = "1",
                            EffectStrength = 3
                        })
                    }
                }
            });

            await this.dbContext.SaveChangesAsync();

            return VoidAppResult.Success();
        }
    }
}