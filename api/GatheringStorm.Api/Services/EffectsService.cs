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
    }

    public interface IEffect
    {
        Task<VoidAppResult> ExecuteEffect(DtoEffectTargets effect, Game game, User currentTurnPlayer,
            CancellationToken cancellationToken = default(CancellationToken));
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
    }
}