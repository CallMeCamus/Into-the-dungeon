using System;
using SadConsole;
using Console = SadConsole.Console;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SadConsole.Components;
using Atestat2._0.UI;
using Atestat2._0.Commands;
using Atestat2._0.Entities;

/*TO DO:
1. FOV implementation
2. Doors, stairs and level progression
3. Revamp the theming to be more aesthetically pleasant and coherent 
4. Monster movement
*/

namespace Atestat2._0
{
  
    class GameLoop
    {
        public const int GameWidth = 100;
        public const int GameHeight = 30;

        public static UIManager UIManager;
        public static World World;
        public static CommandManager CommandManager;

        static void Main(string[] args)
        {
            // Setup the engine and creat the main window.
            SadConsole.Game.Create( GameWidth, GameHeight);
            
            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Hook the update event that happens each frame so we can trap keys and respond.
            SadConsole.Game.OnUpdate = Update;

            // Start the game.
            SadConsole.Game.Instance.Run();

            SadConsole.Game.Instance.Dispose();
        }

        private static void Update(GameTime time)
        {

        }

        private static void Init()
        {
            SadConsole.Game.Instance.Window.Title = "Into the dungeon - Powered by SadConsole";

            //Instantiate the UIManager
            UIManager = new UIManager();

            // Build the world!
            World = new World();

            CommandManager = new CommandManager();
            

            // Now let the UIManager create its consoles
            // so they can use the World data
            UIManager.Init();
        }

    }
}
