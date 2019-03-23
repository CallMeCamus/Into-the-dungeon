using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using System;
using Atestat2._0.Entities;
using SadConsole.Input;
using GoRogue;
using System.Linq;

namespace Atestat2._0.UI
{
    // Creates/Holds/Destroys all consoles used in the game
    // and makes consoles easily addressable from a central place.
    public class UIManager : ContainerConsole
    {
        public ScrollingConsole MapConsole;
        public Window MapWindow;
        public MessageLogWindow MessageLog;
        public StatsWindow StatWindow;

      
        
        //buttons
        private Button closeButton;

        public UIManager()
        {
            // must be set to true
            // or will not call each child's Draw method
            IsVisible = true;
            IsFocused = true;

            

            // The UIManager becomes the only
            // screen that SadConsole processes
            Parent = SadConsole.Global.CurrentScreen;
            
        }

        public void Init()
        {
            CreateConsoles();

            //Message Log initialization
            MessageLog = new MessageLogWindow(GameLoop.GameWidth, GameLoop.GameHeight / 2, "Message Log");
            Children.Add(MessageLog);
            MessageLog.Show();
            MessageLog.Position = new Point(0, GameLoop.GameHeight / 2);


            //Stats console initialization
            StatWindow = new StatsWindow(GameLoop.GameWidth / 2, GameLoop.GameHeight / 2, "Stats");
            
            Children.Add(StatWindow);
            StatWindow.Initialise();
            StatWindow.Refresh();
            StatWindow.Show();
            
            StatWindow.Position = new Point(GameLoop.GameWidth / 2 , 0);
           
            // Load the map into the MapConsole
            LoadMap(GameLoop.World.CurrentMap);

            // Now that the MapConsole is ready, build the Window
            CreateMapWindow(GameLoop.GameWidth / 2, GameLoop.GameHeight / 2, "Game Map");
            UseMouse = true;

            // Start the game with the camera focused on the player
            CenterOnActor(GameLoop.World.Player);
            GameLoop.World.UpdateFOV(GameLoop.World.Player.Position);
        }

        // Creates all child consoles to be managed
        // make sure they are added as children
        // so they are updated and drawn
        public void CreateConsoles()
        {

            // Temporarily create a console with *no* tile data that will later be replaced with map data
            MapConsole = new ScrollingConsole(GameLoop.GameWidth, GameLoop.GameHeight);
    
        }

        private void LoadMap(Map map)
        {
            // First load the map's tiles into the console
            MapConsole = new SadConsole.ScrollingConsole(GameLoop.World.CurrentMap.Width, GameLoop.World.CurrentMap.Width, Global.FontDefault, new Microsoft.Xna.Framework.Rectangle(0, 0, GameLoop.GameWidth, GameLoop.GameHeight), GameLoop.World.CurrentMap.Tiles);

            MapConsole.DefaultBackground = Color.Black;

            // Now Sync all of the map's entities
            SyncMapEntities(map);
        }

        public override void Update(TimeSpan timeElapsed)
        {
            // Called each logic update.
            // As an example, we'll use the F5 key to make the game full screen
            if(CheckKeyboard())
            {
                GameLoop.World.UpdateFOV(GameLoop.World.Player.Position);
            }
            
            base.Update(timeElapsed);
        }

        // Creates a window that encloses a map console
        // of a specified height and width
        // and displays a centered window title
        // make sure it is added as a child of the UIManager
        // so it is updated and drawn
        public void CreateMapWindow(int width, int height, string title)
        {
            MapWindow = new Window(width, height);
            MapWindow.CanDrag = true;
            
            
            //make console short enough to show the window title
            //and borders, and position it away from borders
            int mapConsoleWidth = width - 2;
            int mapConsoleHeight = height - 2;

            // Resize the Map Console's ViewPort to fit inside of the window's borders snugly
            MapConsole.ViewPort = new Microsoft.Xna.Framework.Rectangle(0, 0, mapConsoleWidth, mapConsoleHeight);

            //reposition the MapConsole so it doesnt overlap with the left/top window edges
            MapConsole.Position = new Point(1, 1);

            //close window button
            closeButton = new Button(1, 1);
            closeButton.Position = new Point(0, 0);
            closeButton.Text = "X";

            closeButton.Click += closeButton_click;

            //Add the close button to the Window's list of UI elements
            MapWindow.Add(closeButton);
            

            // Centre the title text at the top of the window
            MapWindow.Title = title.Align(HorizontalAlignment.Center, mapConsoleWidth);

            //add the map viewer to the window
            MapWindow.Children.Add(MapConsole);

            // The MapWindow becomes a child console of the UIManager
            Children.Add(MapWindow);

            // Add the player to the MapConsole's render list
            MapConsole.Children.Add(GameLoop.World.Player);

            // Without this, the window will never be visible on screen
            MapWindow.Show();
            
        }

        private bool CheckKeyboard()
        {
            if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F5))
            {
                SadConsole.Settings.ToggleFullScreen();
                return true;
            }
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down) || SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad2))
            {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(0, 1));
                CenterOnActor(GameLoop.World.Player);
                return true;
            }
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left) || SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad4))
            {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(-1, 0));
                CenterOnActor(GameLoop.World.Player);
                return true;
            }
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Up) || SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad8))
            {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(0, -1));
                CenterOnActor(GameLoop.World.Player);
                return true;
            }
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right) || SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad6))
            {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(1, 0));
                CenterOnActor(GameLoop.World.Player);
                return true;
            }
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad9))
            {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(1, -1));
                CenterOnActor(GameLoop.World.Player);
                return true;
            }
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad3))
            {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(1, 1));
                CenterOnActor(GameLoop.World.Player);
                return true;
            }
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad1))
            {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(-1, 1));
                CenterOnActor(GameLoop.World.Player);
                return true;
            }
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad7))
            {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(-1, -1));
                CenterOnActor(GameLoop.World.Player);
                return true;
            }
            // Undo last command: Z
            if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Z))
            {
                GameLoop.CommandManager.UndoMoveActorBy();
                CenterOnActor(GameLoop.World.Player);
                return true;
            }

            // Repeat last command: X
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.X))
            {
                GameLoop.CommandManager.RedoMoveActorBy();
                CenterOnActor(GameLoop.World.Player);
                return true;
            }
            return false;
        }

        // centers the viewport camera on an Actor
        public void CenterOnActor(Actor actor)
        {
            MapConsole.CenterViewPortOnPoint(actor.Position);
        }

        // Adds the entire list of entities found in the
        // EntityManager's Entities SpatialMap to the
        // MapConsole, so they can be seen onscreen
        private void AddMapEntities()
        {
            foreach(Entity entity in GameLoop.World.CurrentMap.Entities.Items)
            {
                MapConsole.Children.Add(entity);
            }
        }

        // Removes an Entity so it no longer renders onscreen
        public void RemoveEntity(Entity entity)
        {
            MapConsole.Children.Remove(entity);
        }

        // Adds the entire list of entities found in the
        // World.CurrentMap's Entities SpatialMap to the
        // MapConsole, so they can be seen onscreen
        private void SyncMapEntities(Map map)
        {
            // remove all Entities from the console first
            MapConsole.Children.Clear();

            // Now pull all of the entities into the MapConsole in bulk
            foreach(Entity entity in map.Entities.Items)
            {
                MapConsole.Children.Add(entity);
            }

            // Subscribe to the Entities ItemAdded listener, so we can keep our MapConsole entities in sync
            map.Entities.ItemAdded += OnMapEntityAdded;

            // Subscribe to the Entities ItemRemoved listener, so we can keep our MapConsole entities in sync
            map.Entities.ItemRemoved += OnMapEntityRemoved;
        }

        // Remove an Entity from the MapConsole every time the Map's Entity collection changes
        public void OnMapEntityRemoved(object sender, GoRogue.ItemEventArgs<Entity> args)
        {
            MapConsole.Children.Remove(args.Item);
        }

        // Add an Entity to the MapConsole every time the Map's Entity collection changes
        public void OnMapEntityAdded(object sender, GoRogue.ItemEventArgs<Entity> args)
        {
            MapConsole.Children.Remove(args.Item);
        }

        private void closeButton_click(object sender, EventArgs e)
        {
            SadConsole.Game.Instance.Exit();
        }

        
    }
}
