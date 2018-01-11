using System;
using GatheringStorm.Api.Models.DB;
using Newtonsoft.Json;

namespace GatheringStorm.Api.Models.Dto
{
    public class DtoEffect
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TargetsCount { get; set; }

        public static DtoEffect FromDbEffect(CardEffect cardEffect)
        {
            var dtoEffect = new DtoEffect
            {
                Id = cardEffect.Effect.Id,
                Name = cardEffect.Effect.Name
            };
            return dtoEffect;
        }

        private static DtoEffect ConfigureEffect(CardEffect cardEffect, DtoEffect dtoEffect)
        {
            dtoEffect.Description = cardEffect.Effect.Description;
            dtoEffect.TargetsCount = 0;
            return dtoEffect;
        }

        private static DtoEffect ConfigureTargetingEffect(CardEffect cardEffect, DtoEffect dtoEffect)
        {
            var parameters = JsonConvert.DeserializeObject(cardEffect.EffectParameters);
            var targetingText = "";
            if ()
        }
    }
}
