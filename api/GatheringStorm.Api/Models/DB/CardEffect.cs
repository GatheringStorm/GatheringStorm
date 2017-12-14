using System;

namespace GatheringStorm.Api.Models.DB
{
    public class CardEffect
    {
        public Guid Id { get; set; }
        public Effect Effect { get; set; }
        public Card Card { get; set; }
        public string EffectParameters { get; set; }
    }
}
