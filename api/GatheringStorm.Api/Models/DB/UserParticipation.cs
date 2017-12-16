using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GatheringStorm.Api.Models.DB
{
    public class UserParticipation
    {
        private string classId;

        [ForeignKey(nameof(DB.User.Mail))]
        public string Mail { get; set; }
        public User User { get; set; }
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
        public Guid GameId { get; set; }
        public Game Game { get; set; }
    }

    public static class ClassTypeConstants
    {
        private static List<string> classIds = new List<string>
        {
            Swift,
            Medium,
            Tank
        };

        public static string Swift { get; } = "swift";
        public static string Medium { get; } = "medium";
        public static string Tank { get; } = "tank";

        public static bool IsValidClass(this string classId)
        {
            return classIds.Contains(classId);
        }
    }
}
