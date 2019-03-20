using System;
using Microsoft.Xna.Framework;
using SadConsole;

namespace Atestat2._0
{
    // TileBase is an abstract base class 
    // representing the most basic form of of all Tiles used.
    public abstract class TileBase : Cell
    {
        // Movement and Line of Sight Flags
        public bool IsBlockingMove;
        public bool IsTransparent;
        

        // Tile's name
        protected string Name;

        // TileBase is an abstract base class
        // representing the most basic form of of all Tiles used.
        // Every TileBase has a Foreground Colour, Background Colour, and Glyph
        // IsBlockingMove and IsBlockingLOS are optional parameters, set to false by default

        // Default Constructor
        public TileBase(Color foreground, Color background, int glyph, bool blockingMove = false, bool transparent=false, string name = "") : base(foreground, background, glyph)
        {
            IsTransparent = transparent;
            IsBlockingMove = blockingMove;
            IsVisible = false;
            Name = name;
        }

    }
}
