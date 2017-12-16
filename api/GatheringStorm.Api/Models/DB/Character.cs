using System;
using System.Collections.Generic;

namespace GatheringStorm.Api.Models.DB
{
    public class Character
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Card> Cards { get; set; }
    }
}