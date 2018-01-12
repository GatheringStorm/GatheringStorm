using System.Collections.Generic;
using System.Linq;
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
    public interface IDestroyEffect : IEffect
    {
    }

    public class DestroyEffect : IDestroyEffect
    {
        private readonly AppDbContext dbContext;

        public DestroyEffect(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<VoidAppResult> ExecuteEffect(DtoEffectTargets effect, Game game, User currentTurnPlayer,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var cardEffectResult = await this.dbContext.CardEffects.FindEntity(effect.CardEffectId);
            if (cardEffectResult.Result != AppActionResultType.Success)
            {
                return cardEffectResult.GetVoidAppResult();
            }
            var cardEffect = cardEffectResult.SuccessReturnValue;

            var parameters = JsonConvert.DeserializeObject<DestroyEffectParameters>(cardEffect.EffectParameters);

            List<GameCard> targets;
            switch(parameters.TargetingType)
            {
                default: // TODO other cases
                case TargetingType.NumberOfTargets:
                    targets = game.Entities
                        .Where(_ => effect.TargetIds.Contains(_.Id))
                        .Select(_ => _ as GameCard)
                        .Where(_ => _.CardLocation == CardLocation.Board)
                        .ToList();
                    if (targets.Count != effect.TargetIds.Count)
                    {
                        return VoidAppResult.Error(AppActionResultType.UserError, "Some of the targets were invalid targets.");
                    }
                    break;
            }

            foreach (var target in targets)
            {
                target.CardLocation = CardLocation.OutOfPlay;
            }

            throw new System.NotImplementedException();
        }
    }
}