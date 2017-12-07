using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GatheringStorm.Api.Models.DB
{
    public class GameCard : Entity
    {
        public int StatModifiersCount { get; set; }
        public CardLocation CardLocation { get; set; }
    }
}
