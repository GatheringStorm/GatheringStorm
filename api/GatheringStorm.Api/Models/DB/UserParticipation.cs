using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GatheringStorm.Api.Models.DB
{
    public class UserParticipation
    {
        private string classId;

        public string Mail { get; set; }
        public User User { get; set; }
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
        public List<ClassChoice> ClassChoices { get; set; }
        public Guid GameId { get; set; }
        public Game Game { get; set; }
    }
}
