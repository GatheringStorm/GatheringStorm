using System.Runtime.Serialization;

namespace GatheringStorm.Api.Models.DB
{
    public class GameCard : Entity
    {
        public int StatModifiersCount { get; set; }
        public CardLocation CardLocation { get; set; }
        public Card Card { get; set; }
    }

    public enum CardLocation
    {
        [EnumMember(Value = "cellar")]
        Cellar,
        [EnumMember(Value = "hand")]
        Hand,
        [EnumMember(Value = "board")]
        Board,
        [EnumMember(Value = "outOfPlay")]
        OutOfPlay
    }
}
