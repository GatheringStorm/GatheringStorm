using System;
using System.Collections.Generic;
using System.Linq;
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
    public abstract class TargetingEffectBase
    {
        protected readonly AppDbContext dbContext;

        public TargetingEffectBase(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected async Task<AppResult<List<GameCard>>> GetTargetsByIds(DtoEffectTargets effect, TargetingEffectParameters parameters, Game game)
        {
            var targetsCount = Convert.ToInt32(parameters.TargetParameter);
            if (targetsCount != effect.TargetIds.Count)
            {
                return AppResult<List<GameCard>>.Error(AppActionResultType.RuleError, $"You must choose exactly {targetsCount} targets to destroy.");
            }
            var targets = await dbContext.GameCards.IncludeAll()
                .Where(_ => effect.TargetIds.Contains(_.Id))
                .ToListAsync();
            if (targets.Count != effect.TargetIds.Count)
            {
                return VoidAppResult.Error(ErrorPreset.InvalidTargets).GetErrorAppResult<List<GameCard>>();
            }

            return AppResult<List<GameCard>>.Success(targets);
        }

        protected async Task<AppResult<List<GameCard>>> GetTargetsByTitle(DtoEffectTargets effect, TargetingEffectParameters parameters, Game game)
        {
            var title = parameters.TargetParameter.ToString();

            var targets = await dbContext.GameCards.IncludeAll()
                .Where(_ => _.Card.Title.Name == title && _.CardLocation == CardLocation.Board)
                .ToListAsync();

            return AppResult<List<GameCard>>.Success(targets);
        }

        protected async Task<AppResult<List<GameCard>>> GetTargetsByCharacterName(DtoEffectTargets effect, TargetingEffectParameters parameters, Game game)
        {
            var name = parameters.TargetParameter.ToString();

            var targets = await dbContext.GameCards.IncludeAll()
                .Where(_ => _.Card.Character.Name == name && _.CardLocation == CardLocation.Board)
                .ToListAsync();

            return AppResult<List<GameCard>>.Success(targets);
        }

        protected Task<VoidAppResult> ConfigureDtoEffect(CardEffect cardEffect, DtoEffect dtoEffect, string effectPrefix, string effectSuffix)
        {
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