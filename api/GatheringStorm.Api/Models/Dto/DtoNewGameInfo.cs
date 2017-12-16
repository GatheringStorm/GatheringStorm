using System.Collections.Generic;

namespace GatheringStorm.Api.Models.Dto
{
    public class DtoNewGameInfo
    {
        public string OpponentMail { get; set; }
        public List<string> ClassIds { get; set; }
    }
}
