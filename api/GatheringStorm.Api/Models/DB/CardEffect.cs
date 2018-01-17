using System;
using System.Runtime.Serialization;

namespace GatheringStorm.Api.Models.DB
{
    public class CardEffect
    {
        public Guid Id { get; set; }
        public EffectType EffectType { get; set; }
        public Card Card { get; set; }
        public string EffectParameters { get; set; }
    }

    public enum EffectType
    {
        [EnumMember(Value = "destroy")]
        Destroy,
        [EnumMember(Value = "drawCards")]
        DrawCards,
        [EnumMember(Value = "changeStats")]
        ChangeStats
    }
}
