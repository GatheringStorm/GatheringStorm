using System;
using System.Collections.Generic;
using System.Linq;

namespace GatheringStorm.Api.Models.DB
{
    public class ClassChoice
    {
        public ClassType ClassType { get; set; }
        public int Priority { get; set; }

        public string Mail { get; set; }
        public Guid GameId { get; set; }
        public UserParticipation UserParticipation { get; set; }

        public static List<ClassChoice> ChoicesFromClassTypes(List<ClassType> classTypes)
        {
            return classTypes.Select((classType, index) => new ClassChoice
            {
                ClassType = classType,
                Priority = index + 1
            }).ToList();
        }
    }
}