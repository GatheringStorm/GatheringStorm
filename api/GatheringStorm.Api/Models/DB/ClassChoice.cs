using System;

namespace GatheringStorm.Api.Models.DB
{
    public class ClassChoice
    {
        private string classId;

        public string ClassId
        {
            get
            {
                return this.classId;
            }
            set
            {
                if (!value.IsValidClass())
                {
                    throw new ArgumentException("Value is not a valid classId", nameof(ClassId));
                }
                this.classId = value;
            }
        }
        public int Priority { get; set; }
    }
}