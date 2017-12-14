using System;
using System.Collections.Generic;

namespace GatheringStorm.Api.Models.DB
{
    public class Card
    {
        public Guid Id { get; set; }
        public int Cost { get; set; }
        public int Attack { get; set; }
        public int BaseHealth { get; set; }
        public List<CardEffect> Effects { get; set; }
        public List<GameCard> GameCards { get; set; }
        public Title Title { get; set; }
        public Character Character { get; set; }
    }
}
