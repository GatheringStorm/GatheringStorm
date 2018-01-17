using System.Threading;
using System.Threading.Tasks;
using GatheringStorm.Api.Data;
using GatheringStorm.Api.Models;
using GatheringStorm.Api.Models.DB;
using GatheringStorm.Api.Models.Dto;

namespace GatheringStorm.Api.Services.Effects
{
    public interface IDrawCardsEffect : IEffect
    {
    }

    public class DrawCardsEffect : IDrawCardsEffect
    {
        private readonly AppDbContext dbContext;

        public DrawCardsEffect(AppDbContext dbContext)
        {
            this.dbContext = dbContext;

        }

        public Task<VoidAppResult> ConfigureDtoEffect(CardEffect cardEffect, DtoEffect dtoEffect)
        {
            throw new System.NotImplementedException();
        }

        public Task<VoidAppResult> ExecuteEffect(DtoEffectTargets effect, CardEffect cardEffect, Game game, User currentTurnPlayer, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }
    }
}