using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GatheringStorm.Api.Models.DB
{
    public class UserParticipation
    {
        public User User { get; set; }
        public int ClassId { get; set; }
    }
}
