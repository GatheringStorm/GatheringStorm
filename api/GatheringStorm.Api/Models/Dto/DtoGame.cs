using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GatheringStorm.Api.Models.Dto
{
    public class DtoGame
    {
        public Guid Id { get; set; }
        public string CurrentTurnPlayer { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

        [JsonConverter(typeof(StringEnumConverter), true)]
        public DtoGameStatus Status { get; set; }

        public DtoPlayer Opponent { get; set; }
        public DtoPlayer Player { get; set; }
    }

    public enum DtoGameStatus
    {
        [EnumMember(Value = "won")]
        Won,
        [EnumMember(Value = "lost")]
        Lost,
        [EnumMember(Value = "yourTurn")]
        YourTurn,
        [EnumMember(Value = "opponentTurn")]
        OpponentTurn,
        [EnumMember(Value = "invitePending")]
        InvitePending
    }
}
