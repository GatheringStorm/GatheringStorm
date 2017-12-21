using System.Collections.Generic;

namespace GatheringStorm.Api.Models
{
    public static class GameStatusIds
    {
        public static string OpponentInvited { get; } = "opponentInvited";

        private static readonly List<string> dbStatusIds = new List<string>
        {
            DB.InProgress,
            DB.Finished,
            OpponentInvited
        };

        private static readonly List<string> dtoStatusIds = new List<string>
        {
            Dto.YourTurn,
            Dto.OpponentTurn,
            Dto.Won,
            Dto.Lost,
            OpponentInvited
        };

        public class DB
        {
            public static string InProgress { get; } = "inProgress";
            public static string Finished { get; } = "finished";
        }

        public class Dto
        {
            public static string YourTurn { get; } = "yourTurn";
            public static string OpponentTurn { get; } = "opponentTurn";
            public static string Won { get; } = "won";
            public static string Lost { get; } = "lost";
        }

        public static bool IsValidGameStatusId(this string classId, bool db)
        {
            return (db ? dbStatusIds : dtoStatusIds).Contains(classId);
        }
    }
}