using System;

namespace GatheringStorm.Api.Models.Dto
{
    class DtoGame
    {
        public Guid Id { get; set; }
        public string CurrentPlayer { get; set; }
        public DateTime BeginDate { get; set; }

        public DtoPlayer Opponent { get; set; }
        public DtoPlayer Player { get; set; }
    }
}
