using System;
using System.Collections.Generic;

namespace GatheringStorm.Api.Models.Dto
{
    public class DtoBoard
    {
        public Guid Id { get; set; }
        public string CurrentTurnPlayer { get; set; }
        public List<DtoCard> PlayerHandCards { get; set; }
        public int OpponentHandCardsCount { get; set; }
        public DtoBoardPlayer Opponent { get; set; }
        public DtoBoardPlayer Player { get; set; }
    }
}