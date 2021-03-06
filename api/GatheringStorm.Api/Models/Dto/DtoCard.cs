using System;
using System.Collections.Generic;
using System.Linq;
using GatheringStorm.Api.Models.DB;

namespace GatheringStorm.Api.Models.Dto
{
    public class DtoCard
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int Cost { get; set; }
        public bool CanAttack { get; set; }
        public int Attack { get; set; }
        public int Health { get; set; }
        public int StatsModifiersCount { get; set; }
        public List<DtoEffect> Effects { get; set; }
    }
}
