using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GatheringStorm.Api.Models.DB
{
    public class Move
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public MoveType Type { get; set; }
        public Game Game { get; set; }
    }
}
