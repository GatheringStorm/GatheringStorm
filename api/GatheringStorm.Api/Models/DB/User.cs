using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GatheringStorm.Api.Models.DB
{
    public class User : IdentityUser<string>
    {
        public List<Entity> Entities { get; set; }
        public List<UserParticipation> Participations { get; set; }
    }
}
