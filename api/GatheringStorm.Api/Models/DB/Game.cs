using System;
using System.Collections.Generic;

namespace GatheringStorm.Api.Models.DB
{
    public class Game
    {
        private string status;

        public Guid Id { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status
        {
            get => this.status;
            set
            {
                if (!value.IsValidGameStatusId(true))
                {
                    throw new ArgumentException("Value is not a valid gameStatusId: " + value, nameof(Status));
                }
                this.status = value;
            }
        }
        public List<UserParticipation> UserParticipations { get; set; }
        public List<Move> Moves { get; set; }
    }
}
