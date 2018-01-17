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

        public Task<VoidAppResult> ExecuteEffect(DtoEffectTargets effect, Game game, User currentTurnPlayer,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task<VoidAppResult> ConfigureDtoEffect(CardEffect cardEffect, DtoEffect dtoEffect)
        {
            var parameters = JsonConvert.DeserializeObject<ChangeStatsEffectParameters>(cardEffect.EffectParameters);            
            dtoEffect.Name = parameters.EffectStrength > 0 ? "Buff" : "Debuff";
            
            var sign = parameters.EffectStrength > 0 ? "+" : "-";
            return base.ConfigureDtoEffect(cardEffect, dtoEffect, "Give ", $" {sign}{parameters.EffectStrength}/{sign}{parameters.EffectStrength}");
        }
    }
}