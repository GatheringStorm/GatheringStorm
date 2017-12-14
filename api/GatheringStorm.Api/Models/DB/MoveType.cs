using System;
using System.Collections.Generic;

namespace GatheringStorm.Api.Models.DB
{
    public class MoveType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Move> Moves { get; set; }
    }
}
