using System;

namespace GatheringStorm.Api.Models.Dto
{
    public class DtoAttackMove
    {
        public Guid AttackerId { get; set; }
        public Guid TargetId { get; set; }
    }
}