using GatheringStorm.Api.Models.DB;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GatheringStorm.Api.Models.Dto
{
    public class DtoPlayer
    {
        public string Mail { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter), true)]
        public ClassType ClassType { get;set; }
    }
}