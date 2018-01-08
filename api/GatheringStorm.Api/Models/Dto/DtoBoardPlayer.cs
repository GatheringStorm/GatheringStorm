using System.Collections.Generic;

namespace GatheringStorm.Api.Models.Dto
{
    public class DtoBoardPlayer
    {
        public string Mail { get; set; }
        public int Health { get; set; }
        public DtoClass Class { get;set; }
        public List<DtoCard> BoardCards { get; set; }
    }
}