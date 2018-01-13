using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GatheringStorm.Api.Models;
using GatheringStorm.Api.Models.DB;
using GatheringStorm.Api.Models.Dto;
using GatheringStorm.Api.Services.Effects;

namespace GatheringStorm.Api.Services
{
    public interface IEffectsService
    {
        Task<VoidAppResult> ExecuteEffect(DtoEffectTargets effect, Game game, User currentTurnPlayer,
            CancellationToken cancellationToken = default(CancellationToken));
        Task<AppResult<DtoEffect>> GetDtoEffect(CardEffect cardEffect);
    }

    public interface IEffect
    {
        Task<VoidAppResult> ExecuteEffect(DtoEffectTargets effect, Game game, User currentTurnPlayer,
            CancellationToken cancellationToken = default(CancellationToken));
        Task<VoidAppResult> ConfigureDtoEffect(CardEffect cardEffect, DtoEffect dtoEffect);
    }

    public class EffectsService : IEffectsService
    {
        private Dictionary<EffectType, IEffect> effects;

        public EffectsService(IDestroyEffect destroyEffect)
        {
            effects = new Dictionary<EffectType, IEffect>
            {
                [EffectType.Destroy] = destroyEffect
            };
        }

        public async Task<VoidAppResult> ExecuteEffect(DtoEffectTargets effect, Game game, User currentTurnPlayer,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.effects[EffectType.Destroy].ExecuteEffect(effect, game, currentTurnPlayer);
        }

        public async Task<AppResult<DtoEffect>> GetDtoEffect(CardEffect cardEffect)
        {
            var dtoEffect = new DtoEffect
            {
                EffectType = cardEffect.EffectType
            };

            var configureResult = await this.effects[cardEffect.EffectType].ConfigureDtoEffect(cardEffect, dtoEffect);
            if (configureResult.IsErrorResult)
            {
                return configureResult.GetErrorAppResult<DtoEffect>();
            }

            return AppResult<DtoEffect>.Success(dtoEffect);
        }
    }
}