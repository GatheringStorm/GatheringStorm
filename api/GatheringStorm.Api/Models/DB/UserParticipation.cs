using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace GatheringStorm.Api.Models.DB
{
    public class UserParticipation
    {
        public string Mail { get; set; }
        public User User { get; set; }
        public ClassType ClassType { get; set; }
        public List<ClassChoice> ClassChoices { get; set; }
        public Guid GameId { get; set; }
        public Game Game { get; set; }
    }

    public enum ClassType
    {
        Quick,
        Medium,
        Slow
    }
}
