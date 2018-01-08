using System;

namespace GatheringStorm.Api.Models.DB
{
    public class ClassChoice
    {
        public ClassType ClassType { get; set; }
        public int Priority { get; set; }

        public string Mail { get; set; }
        public Guid GameId { get; set; }
        public UserParticipation UserParticipation { get; set; }
    }
}