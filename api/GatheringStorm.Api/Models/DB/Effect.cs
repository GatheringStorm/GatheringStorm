using System;
using System.Collections.Generic;

namespace GatheringStorm.Api.Models.DB
{
    public class Effect
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CardEffect> CardEffect { get; set; }
    }
}
