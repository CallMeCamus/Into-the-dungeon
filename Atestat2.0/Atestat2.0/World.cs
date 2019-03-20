using System;
using SadConsole.Components;
using Microsoft.Xna.Framework;
using Atestat2._0.Entities;
using GoRogue;
using System.Linq;
using Atestat2._0.Entities.Monsters;

namespace Atestat2._0
{
    // All game state data is stored in World
    // also creates and processes generators
    // for map creation
    public class World
    {
        private int _mapWidth = 100;
        private int _mapHeight = 100;
        private int _maxRooms = 20;
        private int _minRoomSize = 6;
        private int _maxRoomSize = 15;
        private TileBase[] _mapTiles; 

        public Map CurrentMap { get; set; }
        public GoRogue.FOV FOVMap;

        public Player Player { get; set; }

        // Creates a new game world and stores it in
        // publicly accessible
        public World()
        {
            // Build a map
            CreateMap();

            // create an instance of player
            CreatePlayer();

            CreateMonsters();
        }

        // Create a new map using the Map class
        // and a map generator. Uses several 
        // parameters to determine geometry
        private void CreateMap()
        {
            _mapTiles = new TileBase[_mapWidth * _mapHeight];
            CurrentMap = new Map(_mapWidth, _mapHeight);
            MapGenerator mapGen = new MapGenerator();
            CurrentMap = mapGen.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);

            var MapView = new GoRogue.MapViews.LambdaMapView<bool>(_mapWidth, _mapHeight, pos => CurrentMap.Tiles[pos.ToIndex(_mapWidth)].IsTransparent);
            FOVMap = new GoRogue.FOV(MapView);
        }

        // Create a player using the Player class
        // and set its starting position
        private void CreatePlayer()
        {
            Player = new Player(Color.Yellow, Color.Transparent);
            // Place the player on the first non-movement-blocking tile on the map
            for(int i = 0; i < CurrentMap.Tiles.Length; i++)
            {
                if(!CurrentMap.Tiles[i].IsBlockingMove)
                {
                    // Set the player's position to the index of the current map position
                    Player.Position = SadConsole.Helpers.GetPointFromIndex(i, CurrentMap.Width);
                }
            }

            // Add the ViewPort sync Component to the player
            Player.Components.Add(new EntityViewSyncComponent());
            
            CurrentMap.Add(Player);
        }

        // Create some random monsters with random attack and defense values
        // and drop them all over the map in
        // random places.
        private void CreateMonsters()
        {
            // number of monsters to create
            int numMonsters = 40;

            // random position generator
            Random rndNum = new Random();

            // Create several monsters and 
            // pick a random position on the map to place them.
            // check if the placement spot is blocking (e.g. a wall)
            // and if it is, try a new position
            for(int i = 0; i < numMonsters; i++)
            {
                int monsterPosition = 0;
                Troll newMonster = new Troll();
                newMonster.Components.Add(new EntityViewSyncComponent());
                while(CurrentMap.Tiles[monsterPosition].IsBlockingMove)
                {
                    // pick a random spot on the map
                    monsterPosition = rndNum.Next(0, CurrentMap.Width * CurrentMap.Height);
                }

                // Set the monster's new position
                // Note: this fancy math will be replaced by a new helper method
                // in the next revision of SadConsole
                newMonster.Position = new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width);
                CurrentMap.Add(newMonster);
            }
        }

        //Update the FOV of the player
        public void UpdateFOV(Point location)
        {
            //Compute FOV at the player's location
            FOVMap.Calculate(location, 10, Radius.CIRCLE);

            //Hide the unseen tiles
            foreach (Coord unseen in FOVMap.NewlyUnseen)
            {
                CurrentMap.Tiles[unseen.ToIndex(CurrentMap.Width)].IsVisible = false;
                foreach(Entity entity in CurrentMap.Entities.Items)
                {
                    if((entity.Position.X == unseen.X)&&(entity.Position.Y == unseen.Y))
                    {
                        entity.Animation.IsVisible = false;
                    }
                }

            }

            //Draw the tiles that are in the FOV
            foreach (Coord seen in FOVMap.NewlySeen)
            {
                CurrentMap.Tiles[seen.ToIndex(CurrentMap.Width)].IsVisible = true;
                foreach (Entity entity in CurrentMap.Entities.Items)
                {
                    if ((entity.Position.X == seen.X) && (entity.Position.Y == seen.Y))
                    {
                        entity.Animation.IsVisible = true;
                    }
                }

            }
            //Make sure the console will update the screen
            GameLoop.UIManager.MapConsole.IsDirty = true;


            GameLoop.UIManager.MessageLog.Add("Seen " + Convert.ToString(GameLoop.World.FOVMap.NewlySeen.Count()));
        }
    }
}
