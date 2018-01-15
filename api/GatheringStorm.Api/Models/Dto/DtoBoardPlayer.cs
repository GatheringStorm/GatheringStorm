using System.Collections.Generic;
using GatheringStorm.Api.Models.DB;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GatheringStorm.Api.Models.Dto
{
    public class DtoBoardPlayer
    {
        public string Mail { get; set; }
        public int Health { get; set; }
        public List<DtoCard> BoardCards { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter), true)]
        public ClassType? ClassType { get;set; }
    }
}