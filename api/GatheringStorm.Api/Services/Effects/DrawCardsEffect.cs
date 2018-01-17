using System.Threading;
using System.Threading.Tasks;
using GatheringStorm.Api.Data;
using GatheringStorm.Api.Models;
using GatheringStorm.Api.Models.DB;
using GatheringStorm.Api.Models.DB.Effects;
using GatheringStorm.Api.Models.Dto;
using Newtonsoft.Json;

namespace GatheringStorm.Api.Services.Effects
{
    public interface IDrawCardsEffect : IEffect
    {
    }

    public class DrawCardsEffect : IDrawCardsEffect
    {
        private readonly AppDbContext dbContext;
        private readonly ICardService cardService;

        public DrawCardsEffect(AppDbContext dbContext, ICardService cardService)
        {
            this.cardService = cardService;
            this.dbContext = dbContext;

        }

        public async Task<VoidAppResult> ExecuteEffect(DtoEffectTargets effect, CardEffect cardEffect, Game game, User currentTurnPlayer, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = JsonConvert.DeserializeObject<DrawCardsEffectParameters>(cardEffect.EffectParameters);

            return await this.cardService.DrawCard(game, currentTurnPlayer.Mail, parameters.CardsCount);
        }

        public Task<VoidAppResult> ConfigureDtoEffect(CardEffect cardEffect, DtoEffect dtoEffect)
        {
            var parameters = JsonConvert.DeserializeObject<DrawCardsEffectParameters>(cardEffect.EffectParameters);

            dtoEffect.Name = "Draw cards";
            dtoEffect.TargetsCount = 0;
            dtoEffect.Description = $"Draw {parameters.CardsCount} cards.";

            return Task.FromResult(VoidAppResult.Success());
        }
    }
}