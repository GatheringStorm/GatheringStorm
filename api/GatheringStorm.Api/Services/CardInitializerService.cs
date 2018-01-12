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
        Task<VoidAppResult> InitializeGame(Game game);
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
                Id = Guid.NewGuid(),
                Cost = 1,
                Attack = 2,
                BaseHealth = 1,
                IsLegendary = false,
                Title = pawn,
                Character = claus
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 1,
                Attack = 1,
                BaseHealth = 2,
                IsLegendary = false,
                Title = pawn,
                Character = claus
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Cost = 5,
                Attack = 4,
                BaseHealth = 4,
                IsLegendary = true, // TODO: CHANGE THIS! ONLY FOR TESTING!
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

        public async Task<VoidAppResult> InitializeGame(Game game)
        {
            foreach (var participation in game.UserParticipations)
            {
                foreach (var card in this.dbContext.Cards)
                {
                    var cardResult = await this.CreateGameCard(card.Id, participation.User, card.IsLegendary ? 1 : 7);
                    if (cardResult.Result != AppActionResultType.Success)
                    {
                        return cardResult.GetVoidAppResult();
                    }

                    game.Entities.AddRange(cardResult.SuccessReturnValue);
                }
            }

            return VoidAppResult.Success();
        }

        private async Task<AppResult<List<GameCard>>> CreateGameCard(Guid cardId, User user, int duplicatesCount)
        {
            var cardResult = await this.dbContext.Cards.FindEntity(cardId);
            if (cardResult.Result != AppActionResultType.Success)
            {
                return cardResult.GetVoidAppResult().GetErrorAppResult<List<GameCard>>();
            }
            var cards = new List<GameCard>();
            var card = cardResult.SuccessReturnValue;
            for(var i = 0; i < duplicatesCount; i++)
            {
                cards.Add(new GameCard
                {
                    Id = Guid.NewGuid(),
                    Health = card.BaseHealth,
                    User = user,
                    Card = card,
                    CardLocation = CardLocation.Cellar
                });
            }
            return AppResult<List<GameCard>>.Success(cards);
        }
    }
}