using System.Collections.Generic;

namespace GatheringStorm.Api.Models
{
    public static class ClassTypes
    {
        public static string Swift { get; } = "swift";
        public static string Medium { get; } = "medium";
        public static string Tank { get; } = "tank";

        private static List<string> classIds = new List<string>
        {
            Swift,
            Medium,
            Tank
        };

        public static bool IsValidClass(this string classId)
        {
            return classId == null ||  classIds.Contains(classId);
        }
    }
}
