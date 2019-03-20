using System;
using Microsoft.Xna.Framework;

namespace Atestat2._0
{
    // TileFloor is based on TileBase
    // Floor tiles to be used in maps.
    public class TileFloor : TileBase
    {
        // Default constructor
        // Floors are set to allow movement and line of sight by default
        // and have a dark gray foreground and a transparent background
        // represented by the . symbol
        public TileFloor(bool blocksMovement = false, bool transparent = true) : base(Color.DarkGray, Color.Transparent, '.', blocksMovement, transparent)
        {
            Name = "Floor";
        }
    }
}
