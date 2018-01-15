using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GatheringStorm.Api.Data;
using GatheringStorm.Api.Models;
using GatheringStorm.Api.Models.DB;
using GatheringStorm.Api.Models.DB.Effects;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GatheringStorm.Api.Services
{
    public interface ICardInitializerService
    {
        Task<VoidAppResult> InitializeCards();
        Task<VoidAppResult> InitializeGame(Game game);
        Task<AppResult<GameCard>> GenerateStormling(User user);
    }

    public class CardInitializerService : ICardInitializerService
    {
        private readonly AppDbContext dbContext;
        private static readonly Guid StormlingId = Guid.Parse("53746f72-6d6c-696e-6700-000000000000");

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
                var cards = new List<GameCard>();
                foreach (var card in this.dbContext.Cards)
                {
                    var cardResult = await this.CreateGameCard(card.Id, participation.User, card.IsLegendary ? 1 : 7);
                    if (cardResult.IsErrorResult)
                    {
                        return cardResult.GetVoidAppResult();
                    }

                    cards.AddRange(cardResult.SuccessReturnValue);
                    game.Entities.AddRange(cardResult.SuccessReturnValue);
                }

                var handCardIndizes = new List<int>();
                var random = new Random();
                while (handCardIndizes.Count < 3)
                {
                    var newIndex = random.Next(cards.Count);
                    if (handCardIndizes.Contains(newIndex))
                    {
                        continue;
                    }
                    cards[newIndex].CardLocation = CardLocation.Hand;
                    handCardIndizes.Add(newIndex);
                }
            }

            return VoidAppResult.Success();
        }

        public async Task<AppResult<GameCard>> GenerateStormling(User user)
        {
            var stormlingCard = await this.dbContext.Cards.SingleOrDefaultAsync(_ => _.Id == StormlingId);
            if (stormlingCard == null)
            {
                return VoidAppResult.Error(ErrorPreset.OnLoadingData).GetErrorAppResult<GameCard>();
            }

            var stormling = new GameCard
            {
                Id = Guid.NewGuid(),
                Health = stormlingCard.BaseHealth,
                Card = stormlingCard,
                CardLocation = CardLocation.Board,
                User = user
            };
            return AppResult<GameCard>.Success(stormling);
        }

        private async Task<AppResult<List<GameCard>>> CreateGameCard(Guid cardId, User user, int duplicatesCount)
        {
            var card = await this.dbContext.Cards.SingleOrDefaultAsync(_ => _.Id == cardId);
            if (card == null)
            {
                return VoidAppResult.Error(ErrorPreset.OnLoadingData).GetErrorAppResult<List<GameCard>>();
            }
            var cards = new List<GameCard>();
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