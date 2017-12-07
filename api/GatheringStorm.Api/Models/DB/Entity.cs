using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GatheringStorm.Api.Models.DB
{
    public class Entity
    {
        public Guid Id { get; set; }
        public int Health { get; set; }
        public List<MoveTargetEntity> TargetingMoves { get; set; }
    }
}
