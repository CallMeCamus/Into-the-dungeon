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

        }
    }
}
