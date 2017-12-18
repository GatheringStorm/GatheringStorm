using System;

namespace GatheringStorm.Api.Models.DB
{
    public class ClassChoice
    {
        private string classId;

        public string ClassId
        {
            get => this.classId;
            set
            {
                if (!value.IsValidClass())
                {
                    throw new ArgumentException("Value is not a valid classId: " + value, nameof(ClassId));
                }
                this.classId = value;
            }
        }
        public int Priority { get; set; }

        public string Mail { get; set; }
        public Guid GameId { get; set; }
        public UserParticipation UserParticipation { get; set; }
    }
}