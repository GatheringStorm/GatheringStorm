﻿using System;
using System.Collections.Generic;

namespace GatheringStorm.Api.Models.DB
{
    public class Entity
    {
        public Guid Id { get; set; }
        public int Health { get; set; }
        public User User { get; set; }
        public List<MoveTargetEntity> TargetingMoves { get; set; }
        public Game Game { get; set; }
    }
}
