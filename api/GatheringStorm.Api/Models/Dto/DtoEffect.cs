using System;

namespace GatheringStorm.Api.Models.Dto
{
    public class DtoEffect
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TargetsCount { get; set; }
    }
}
