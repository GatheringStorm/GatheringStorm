using System;
using System.Collections.Generic;

namespace GatheringStorm.Api.Models.Dto
{
    public class DtoBoard
    {
        public Guid Id { get; set; }
        public string CurrentPlayer { get; set; }
        public int PlayerHealth { get; set; }
        public int OpponentHealth { get; set; }
        public List<DtoCard> PlayerHandCards { get; set; }
        public int OpponentHandCardsCount { get; set; }
        public List<DtoCard> PlayerBoardCards { get; set; }
        public List<DtoCard> OpponentBoardCards { get; set; }
    }
}