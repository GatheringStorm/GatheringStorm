using System.Collections.Generic;
using GatheringStorm.Api.Models.DB;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GatheringStorm.Api.Models.Dto
{
    public class DtoNewGameInfo
    {
        public string OpponentMail { get; set; }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter),
            ItemConverterParameters = new object[] { true })]
        public List<ClassType> ClassTypes { get; set; }
    }
}
