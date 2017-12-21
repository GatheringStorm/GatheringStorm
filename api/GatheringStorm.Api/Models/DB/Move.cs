using System;
using System.Collections.Generic;

namespace GatheringStorm.Api.Models.DB
{
    public class Move
    {
        private string moveType;

        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Type
        {
            get
            {
                return this.moveType;
            } 
            set
            {
                if (!value.IsValidMoveType())
                {
                    throw new ArgumentException("Value is not a valid classId: " + value, nameof(Type));
                }

                this.moveType = value;
            }
        }
        public Game Game { get; set; }
        public Entity SourceEntity { get; set; }
        public List<MoveTargetEntity> TargetEntities { get; set; }
    }
}
