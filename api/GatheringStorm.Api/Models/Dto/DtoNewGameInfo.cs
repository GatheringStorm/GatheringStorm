using System;

namespace GatheringStorm.Api.Models.Dto
{
    public class DtoNewGameInfo
    {
        public Guid OpponentId { get; set; }
        public int ClassId { get; set; }
    }
}
