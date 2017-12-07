using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GatheringStorm.Api.Models.DB
{
    public class User
    {
        public string Mail { get; set; }
        public List<Entity> Entities { get; set; }
        public List<UserParticipation> Participations { get; set; }
    }
}
