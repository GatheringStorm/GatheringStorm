namespace GatheringStorm.Api.Models.DB.Effects
{
    public class TargetingEffectParameters
    {
        public TargetingType TargetingType { get; set; }
        public string TargetParameter { get; set; }
    }

    public enum TargetingType
    {
        NumberOfTargets,
        CharacterName,
        Title
    }
}