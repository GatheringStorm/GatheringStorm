using System;

namespace GatheringStorm.Api.Models.Dto
{
    public class DtoGame
    {
        private string status;

        public Guid Id { get; set; }
        public string CurrentTurnPlayer { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status
        {
            get => this.status;
            set
            {
                if (!value.IsValidGameStatusId(false))
                {
                    throw new ArgumentException("Value is not a valid gameStatusId: " + value, nameof(Status));
                }
                this.status = value;
            }
        }

        public DtoPlayer Opponent { get; set; }
        public DtoPlayer Player { get; set; }
    }
}
