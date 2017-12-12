using System;

namespace GatheringStorm.Api.Models.DB
{
    public class MoveTargetEntity
    {
        public Guid MoveId { get; set; }
        public Move Move { get; set; }
        public Guid EntityId { get; set; }
        public Entity Entity { get; set; }
    }
}
