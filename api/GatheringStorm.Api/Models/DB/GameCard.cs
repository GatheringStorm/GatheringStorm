namespace GatheringStorm.Api.Models.DB
{
    public class GameCard : Entity
    {
        public int StatModifiersCount { get; set; }
        public CardLocation CardLocation { get; set; }
    }
}
