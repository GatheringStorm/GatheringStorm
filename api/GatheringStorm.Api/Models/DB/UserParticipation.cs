using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GatheringStorm.Api.Models.DB
{
    public class UserParticipation
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public int ClassId { get; set; }
        public Guid GameId { get; set; }
        public Game Game { get; set; }
    }
}
