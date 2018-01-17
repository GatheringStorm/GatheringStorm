using System;
using GatheringStorm.Api.Models.DB;
using Newtonsoft.Json;

namespace GatheringStorm.Api.Models.Dto
{
    public class DtoEffect
    {
        public Guid Id { get; set; }
        public EffectType EffectType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TargetsCount { get; set; }
    }
}
