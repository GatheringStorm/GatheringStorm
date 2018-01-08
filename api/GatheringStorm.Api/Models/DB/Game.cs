using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GatheringStorm.Api.Models.DB
{
    public class Game
    {
        public Guid Id { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public GameStatus Status { get; set; }
        public List<UserParticipation> UserParticipations { get; set; }
        public List<Move> Moves { get; set; }
        public List<Entity> Entities { get; set; }
    }

    public enum GameStatus
    {
        Finished,
        InvitePending,
        InProgress
    }
}
