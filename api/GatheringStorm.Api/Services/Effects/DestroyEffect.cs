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

            AppResult<List<GameCard>> targetsResult;
            switch(parameters.TargetingType)
            {
                case TargetingType.Title:
                    targetsResult = this.GetTargetsByTitle(effect, parameters, game);
                    break;
                case TargetingType.CharacterName:
                    targetsResult = this.GetTargetsByCharacterName(effect, parameters, game);
                    break;
                default: // TargetingType.NumberOfTargets
                    targetsResult = this.GetTargetsByIds(effect, parameters, game);
                    break;
            }
            if (targetsResult.Result != AppActionResultType.Success)
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

            throw new System.NotImplementedException();
        }

        private AppResult<List<GameCard>> GetTargetsByIds(DtoEffectTargets effect, DestroyEffectParameters parameters, Game game)
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

        private AppResult<List<GameCard>> GetTargetsByTitle(DtoEffectTargets effect, DestroyEffectParameters parameters, Game game)
        {
            var title = parameters.TargetParameter.ToString();

            var targets = game.Entities
                .Select(_ => _ as GameCard)
                .Where(_ => _ != null && _.Card.Title.Name == title)
                .ToList();

            return AppResult<List<GameCard>>.Success(targets);
        }

        private AppResult<List<GameCard>> GetTargetsByCharacterName(DtoEffectTargets effect, DestroyEffectParameters parameters, Game game)
        {
            var name = parameters.TargetParameter.ToString();

            var targets = game.Entities
                .Select(_ => _ as GameCard)
                .Where(_ => _ != null && _.Card.Character.Name == name)
                .ToList();

            return AppResult<List<GameCard>>.Success(targets);
        }

        public Task<VoidAppResult> ConfigureDtoEffect(CardEffect cardEffect, DtoEffect dtoEffect)
        {
            dtoEffect.Name = "Destroy";

            var parameters = JsonConvert.DeserializeObject<DestroyEffectParameters>(cardEffect.EffectParameters);
            dtoEffect.TargetsCount = parameters.TargetingType == TargetingType.NumberOfTargets
                ? Convert.ToInt32(parameters.TargetParameter)
                : 0;

            switch (parameters.TargetingType)
            {
                case TargetingType.Title:
                    dtoEffect.Description = $"Destroy all cards with the title '{parameters.TargetParameter}'.";
                    break;
                case TargetingType.CharacterName:
                    dtoEffect.Description = $"Destroy all '{parameters.TargetParameter}'.";
                    break;
                default: // TargetingType.NumberOfTargets
                    dtoEffect.Description = $"Destroy {parameters.TargetParameter} cards.";
                    break;
            }

            return Task.FromResult(VoidAppResult.Success());
        }
    }
}