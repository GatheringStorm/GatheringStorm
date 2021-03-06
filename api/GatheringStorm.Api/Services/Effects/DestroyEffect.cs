using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GatheringStorm.Api.Data;
using GatheringStorm.Api.Models;
using GatheringStorm.Api.Models.DB;
using GatheringStorm.Api.Models.DB.Effects;
using GatheringStorm.Api.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GatheringStorm.Api.Services.Effects
{
    public interface IDestroyEffect : IEffect
    {
    }

    public class DestroyEffect : TargetingEffectBase, IDestroyEffect
    {
        public DestroyEffect(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<VoidAppResult> ExecuteEffect(DtoEffectTargets effect, CardEffect cardEffect, Game game, User currentTurnPlayer,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = JsonConvert.DeserializeObject<DestroyEffectParameters>(cardEffect.EffectParameters);

            AppResult<List<GameCard>> targetsResult;
            switch(parameters.TargetingType)
            {
                case TargetingType.Title:
                    targetsResult = await base.GetTargetsByTitle(effect, parameters, game);
                    break;
                case TargetingType.CharacterName:
                    targetsResult = await base.GetTargetsByCharacterName(effect, parameters, game);
                    break;
                default: // TargetingType.NumberOfTargets
                    targetsResult = await base.GetTargetsByIds(effect, parameters, game);
                    break;
            }
            if (targetsResult.IsErrorResult)
            {
                return targetsResult.GetVoidAppResult();
            }
            var targets = targetsResult.SuccessReturnValue;
            if (targets.Any(_ => _.CardLocation != CardLocation.Board))
            {
                return VoidAppResult.Error(ErrorPreset.InvalidTargets);
            }

            foreach (var target in targets)
            {
                target.CardLocation = CardLocation.OutOfPlay;
            }

            return VoidAppResult.Success();
        }

        public Task<VoidAppResult> ConfigureDtoEffect(CardEffect cardEffect, DtoEffect dtoEffect)
        {
            dtoEffect.Name = "Destroy";

            return base.ConfigureDtoEffect(cardEffect, dtoEffect, "Destroy ", "");
        }
    }
}