using System;
using System.Collections.Generic;

namespace GatheringStorm.Api.Models.DB
{
    public class CardLocation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<GameCard> GameCards { get; set; }
    }
}
