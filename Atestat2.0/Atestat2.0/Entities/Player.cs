﻿using System;
using Microsoft.Xna.Framework;

namespace Atestat2._0.Entities
{
    public class Player : Actor
    {
        public Player(Color foreground, Color background) : base (foreground,background, '@')
        {
            Attack = 10;
            AttackChance = 40;
            Defense = 5;
            DefenseChance = 20;
            Gold = 0;
            Name = "Homer";
        }
    }
}
