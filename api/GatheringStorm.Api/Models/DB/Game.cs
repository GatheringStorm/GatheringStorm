using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GatheringStorm.Api.Models.DB
{
    public class Game
    {
        public Guid Id { get; set; }
        public DateTime BeginDate { get; set; }
        public bool IsFinished { get; set; }
        public List<UserParticipation> UserParticipations { get; set; }
        public List<Move> Moves { get; set; }
    }
}
