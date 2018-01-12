namespace GatheringStorm.Api.Models.DB.Effects
{
    public class DestroyEffectParameters
    {
        public TargetingType TargetingType { get; set; }
        public object TargetParameter { get; set; }
    }

    public enum TargetingType
    {
        NumberOfTargets,
        CharacterName,
        Title
    }
}