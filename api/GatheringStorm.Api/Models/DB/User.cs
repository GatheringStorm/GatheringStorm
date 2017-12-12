using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GatheringStorm.Api.Models.DB
{
    public class User
    {
        [Key]
        public string Mail { get; set; }
        public List<Entity> Entities { get; set; }
        public List<UserParticipation> Participations { get; set; }
    }
}
