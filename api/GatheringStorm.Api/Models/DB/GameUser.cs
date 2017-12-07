using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GatheringStorm.Api.Models.DB
{
    public class GameUser
    {
        public User User1 { get; set; }
        public User User2 { get; set; }
        public int ClassIdUser1 { get; set; }
        public int ClassIdUser2 { get; set; }
        public List<Game> Games { get; set; }
    }
}
