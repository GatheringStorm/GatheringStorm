using System;
using System.Collections.Generic;

namespace GatheringStorm.Api.Models.Dto
{
    public class DtoEffectTargets
    {
        public Guid CardEffectId { get; set; }
        public List<Guid> TargetIds { get; set; }
    }
}