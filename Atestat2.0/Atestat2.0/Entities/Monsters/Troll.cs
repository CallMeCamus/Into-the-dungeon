using System;
using Microsoft.Xna.Framework;

namespace Atestat2._0.Entities.Monsters
{
    //A simple monster class
    public class Troll : Monster
    {
        Random rndNum = new Random();
        
        public Troll() : base(Color.Red, Color.Transparent, 'M')
        {
            Health = rndNum.Next(0, 10);
            Defense = rndNum.Next(0, 10);
            DefenseChance = rndNum.Next(0, 50);
            Attack = rndNum.Next(0, 10);
            AttackChance = rndNum.Next(0, 50);
            Gold = rndNum.Next(0, 10);
            Name = "a common troll";
        }
    }
}
