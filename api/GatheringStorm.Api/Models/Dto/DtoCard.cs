using System;
using System.Collections.Generic;
using System.Linq;
using GatheringStorm.Api.Models.DB;

namespace GatheringStorm.Api.Models.Dto
{
    public class DtoCard
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int Cost { get; set; }
        public bool CanAttack { get; set; }
        public int Attack { get; set; }
        public int Health { get; set; }
        public int StatsModifiersCount { get; set; }
        public List<DtoEffect> Effects { get; set; }

        public static DtoCard FromDbCard(GameCard gameCard)
        {
            return new DtoCard
            {
                Id = gameCard.Id,
                Name = gameCard.Card.Character.Name,
                Title = gameCard.Card.Title.Name,
                Cost = gameCard.Card.Cost,
                Attack = gameCard.Card.Attack,
                CanAttack = false,
                Health = gameCard.Health,
                StatsModifiersCount = gameCard.StatModifiersCount,
                Effects = gameCard.Card.Effects
                    .Select(ce => DtoEffect.FromDbEffect(ce.Effect))
                    .ToList()
            };
        }
    }
}
