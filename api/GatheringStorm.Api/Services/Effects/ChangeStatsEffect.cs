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
    public interface IChangeStatsEffect : IEffect
    {
    }

    public class ChangeStatsEffect : TargetingEffectBase, IChangeStatsEffect
    {
        public ChangeStatsEffect(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<VoidAppResult> ExecuteEffect(DtoEffectTargets effect, CardEffect cardEffect, Game game, User currentTurnPlayer,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = JsonConvert.DeserializeObject<ChangeStatsEffectParameters>(cardEffect.EffectParameters);

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
                target.Health += parameters.EffectStrength;
                target.StatModifiersCount += parameters.EffectStrength;

                if (target.Health <= 0)
                {
                    target.CardLocation = CardLocation.OutOfPlay;
                }
            }

            return VoidAppResult.Success();
        }

        public Task<VoidAppResult> ConfigureDtoEffect(CardEffect cardEffect, DtoEffect dtoEffect)
        {
            var parameters = JsonConvert.DeserializeObject<ChangeStatsEffectParameters>(cardEffect.EffectParameters);            
            dtoEffect.Name = parameters.EffectStrength > 0 ? "Buff" : "Debuff";
            
            var sign = parameters.EffectStrength > 0 ? "+" : "";
            return base.ConfigureDtoEffect(cardEffect, dtoEffect, "Give ", $" {sign}{parameters.EffectStrength}/{sign}{parameters.EffectStrength}");
        }
    }
}