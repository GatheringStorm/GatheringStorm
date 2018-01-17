using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GatheringStorm.Api.Data;
using GatheringStorm.Api.Models;
using GatheringStorm.Api.Models.DB;
using GatheringStorm.Api.Models.Dto;
using GatheringStorm.Api.Services.Effects;
using Microsoft.EntityFrameworkCore;

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
        Task<VoidAppResult> ExecuteEffect(DtoEffectTargets effect, CardEffect cardEffect, Game game, User currentTurnPlayer,
            CancellationToken cancellationToken = default(CancellationToken));
        Task<VoidAppResult> ConfigureDtoEffect(CardEffect cardEffect, DtoEffect dtoEffect);
    }

    public class EffectsService : IEffectsService
    {
        private Dictionary<EffectType, IEffect> effects;
        private readonly AppDbContext dbContext;

        public EffectsService(IDestroyEffect destroyEffect, IChangeStatsEffect changeStatsEffect, IDrawCardsEffect drawCardsEffect, AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            
            effects = new Dictionary<EffectType, IEffect>
            {
                [EffectType.Destroy] = destroyEffect,
                [EffectType.DrawCards] = drawCardsEffect,
                [EffectType.ChangeStats] = changeStatsEffect
            };
        }

        public async Task<VoidAppResult> ExecuteEffect(DtoEffectTargets effect, Game game, User currentTurnPlayer,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var cardEffect = await this.dbContext.CardEffects.SingleOrDefaultAsync(_ => _.Id == effect.CardEffectId);
            if (cardEffect == null)
            {
                return VoidAppResult.Error(ErrorPreset.OnLoadingData);
            }

            return await this.effects[cardEffect.EffectType].ExecuteEffect(effect, cardEffect, game, currentTurnPlayer);
        }

        public async Task<AppResult<DtoEffect>> GetDtoEffect(CardEffect cardEffect)
        {
            var dtoEffect = new DtoEffect
            {
                Id = cardEffect.Id,
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