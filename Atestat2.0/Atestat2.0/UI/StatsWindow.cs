using System.Collections.Generic;
using SadConsole;
using System;
using Microsoft.Xna.Framework;


namespace Atestat2._0.UI
{
    //A window that displays player's stats
    public class StatsWindow : Window
    {
        private SadConsole.Console _statConsole;

        public enum Stats { Name, Attack, AttackChance, Defense, DefenseChance, Gold};
        
        public readonly List<ColoredString> _lines;

        // account for the thickness of the window border to prevent UI element spillover
        private int _windowBorderThickness = 2;

        public StatsWindow(int width, int height, string title) : base(width, height)
        {
            //Theme.WindowTheme.FillStyle.Background = DefaultBackground;
            
            Title = title.Align(HorizontalAlignment.Center, width);
            _lines = new List<ColoredString>();
            CanDrag = true;

            _statConsole = new SadConsole.Console(width - _windowBorderThickness, height - _windowBorderThickness);
            _statConsole.Position = new Point(1, 1);
            _statConsole.DefaultBackground = Color.Black;
            

            Children.Add(_statConsole);
        }

        public void Initialise()
        {
            //Initialise the _lines list with all the player's stats
            //using coloredstrings because it's pretty
            ColoredString stat;
            stat = new ColoredString($"Name: {GameLoop.World.Player.Name}", Color.Green, Color.Transparent);
            _lines.Add(stat);
            stat = new ColoredString($"Attack: {GameLoop.World.Player.Attack}", Color.Red, Color.Transparent);
            _lines.Add(stat);
            stat = new ColoredString($"AttackChance: {GameLoop.World.Player.AttackChance}", Color.Blue, Color.Transparent);
            _lines.Add(stat);
            stat = new ColoredString($"Defense: {GameLoop.World.Player.Defense}", Color.DarkGoldenrod, Color.Transparent);
            _lines.Add(stat);
            stat = new ColoredString($"DefenseChance: {GameLoop.World.Player.DefenseChance}", Color.Blue, Color.Transparent);
            _lines.Add(stat);
            stat = new ColoredString($"Gold: {GameLoop.World.Player.Gold}", Color.Gold, Color.Transparent);
            _lines.Add(stat);
            
        }

        public void Refresh()
        {
            //printing the stats on the console
            //note about coloredstrings: don't use Cursor.PrintOnlyCharacterData = true if you want to see color
            //note 2 about colored strings _statConsole.Cursor.Print(message + "\r\n\"); will print without color because of the "+" operator
            foreach (ColoredString message in _lines)
            {
                
                _statConsole.Cursor.Position = new Point(1, _lines.IndexOf(message));
                _statConsole.Cursor.Print(message);
            }
            
        }

        //Remember to draw the window!
        public override void Draw(TimeSpan drawTime)
        {
            base.Draw(drawTime);
        }
    }
}
