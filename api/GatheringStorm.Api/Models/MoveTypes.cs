using System.Collections.Generic;

namespace GatheringStorm.Api.Models
{
    public static class MoveTypes
    {
        public static string EndTurn { get; } = "endTurn";

        public static string Attack { get; } = "attack";

        private static readonly List<string> moveTypes = new List<string>
        {
            EndTurn,
            Attack
        };

        public static bool IsValidMoveType(this string moveType)
        {
            return moveTypes.Contains(moveType);
        }
    }
}
