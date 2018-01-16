using System;
using System.Collections.Generic;

namespace GatheringStorm.Api.Models.Dto
{
    public class DtoPlayCardMove
    {
        public Guid CardId { get; set; }
        public List<Guid> DiscardedCardIds { get; set; }
        public List<DtoEffectTargets> EffectTargets { get; set; }
    }
}
