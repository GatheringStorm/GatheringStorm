using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GatheringStorm.Api.Data;
using GatheringStorm.Api.Models;
using GatheringStorm.Api.Models.DB;
using GatheringStorm.Api.Models.DB.Effects;
using GatheringStorm.Api.Models.Dto;
using Newtonsoft.Json;

namespace GatheringStorm.Api.Services.Effects
{
    public abstract class TargetingEffectBase
    {
        protected readonly AppDbContext dbContext;

        public TargetingEffectBase(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected AppResult<List<GameCard>> GetTargetsByIds(DtoEffectTargets effect, DestroyEffectParameters parameters, Game game)
        {
            var targetsCount = Convert.ToInt32(parameters.TargetParameter);
            if (targetsCount != effect.TargetIds.Count)
            {
                return AppResult<List<GameCard>>.Error(AppActionResultType.RuleError, $"You must choose exactly {targetsCount} targets to destroy.");
            }
            var targets = game.Entities
                .Where(_ => effect.TargetIds.Contains(_.Id))
                .Select(_ => _ as GameCard)
                .ToList();
            if (targets.Count != effect.TargetIds.Count)
            {
                return VoidAppResult.Error(ErrorPreset.InvalidTargets).GetErrorAppResult<List<GameCard>>();
            }

            return AppResult<List<GameCard>>.Success(targets);
        }

        protected AppResult<List<GameCard>> GetTargetsByTitle(DtoEffectTargets effect, DestroyEffectParameters parameters, Game game)
        {
            var title = parameters.TargetParameter.ToString();

            var targets = game.Entities
                .Select(_ => _ as GameCard)
                .Where(_ => _ != null && _.Card.Title.Name == title)
                .ToList();

            return AppResult<List<GameCard>>.Success(targets);
        }

        protected AppResult<List<GameCard>> GetTargetsByCharacterName(DtoEffectTargets effect, DestroyEffectParameters parameters, Game game)
        {
            var name = parameters.TargetParameter.ToString();

            var targets = game.Entities
                .Select(_ => _ as GameCard)
                .Where(_ => _ != null && _.Card.Character.Name == name)
                .ToList();

            return AppResult<List<GameCard>>.Success(targets);
        }

        protected Task<VoidAppResult> ConfigureDtoEffect(CardEffect cardEffect, DtoEffect dtoEffect, string effectPrefix, string effectSuffix)
        {
            dtoEffect.Name = "Destroy";

            var parameters = JsonConvert.DeserializeObject<TargetingEffectParameters>(cardEffect.EffectParameters);
            dtoEffect.TargetsCount = parameters.TargetingType == TargetingType.NumberOfTargets
                ? Convert.ToInt32(parameters.TargetParameter)
                : 0;

            switch (parameters.TargetingType)
            {
                case TargetingType.Title:
                    dtoEffect.Description = $"{effectPrefix}all cards with the title '{parameters.TargetParameter}'{effectSuffix}.";
                    break;
                case TargetingType.CharacterName:
                    dtoEffect.Description = $"{effectPrefix}all '{parameters.TargetParameter}'{effectSuffix}.";
                    break;
                default: // TargetingType.NumberOfTargets
                    dtoEffect.Description = $"{effectPrefix}{parameters.TargetParameter} cards{effectSuffix}.";
                    break;
            }

            return Task.FromResult(VoidAppResult.Success());
        }
    }
}