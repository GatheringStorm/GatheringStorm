using System;
using System.Collections.Generic;
using GatheringStorm.Api.Models.DB;

namespace GatheringStorm.Api.Controllers
{
    public class DtoJoinGameInfo
    {
        public Guid GameId { get; set; }
        public List<ClassType> ClassTypes { get; set; }
    }
}