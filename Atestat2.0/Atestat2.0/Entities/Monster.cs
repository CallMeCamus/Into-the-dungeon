using System;
using Microsoft.Xna.Framework;

namespace Atestat2._0.Entities
{
    //A generic monster capable of
    //combat and interaction
    //yields treasure upon death
    public class Monster : Actor
    {
        public Monster(Color foreground , Color background, int glyph) : base(foreground, background, glyph)
        {
            Random rndNum = new Random();

            //number of loot to spawn for monster
            int numLoot = rndNum.Next(1, 4);

            for(int i = 0; i < numLoot; i++)
            {
                // monsters are made out of spork, obvs.
                Item newLoot = new Item(Color.HotPink, Color.Transparent, "spork", 'L', 2);
                
                Inventory.Add(newLoot);
            }
        }
    }
}
