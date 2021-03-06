﻿using System;
using Microsoft.Xna.Framework;
using Atestat2._0.Entities;
using System.Text;
using GoRogue.DiceNotation;
using SadConsole;

namespace Atestat2._0.Commands
{
    // Contains all generic actions performed on entities and tiles
    // including combat, movement, and so on.
    public class CommandManager
    {
        //stores the actor's last move action
        private Point _lastActorPoint;
        private Actor _lastMoveActor;
        

        public CommandManager()
        {
            
        }

        // Move the actor BY +/- X&Y coordinates
        // returns true if the move was successful
        // and false if unable to move there
        public bool MoveActorBy(Actor actor, Point position)
        {
            _lastMoveActor = actor;
            _lastActorPoint = position;

            return actor.MoveBy(position);
        }

        // Redo last actor move
        public bool RedoMoveActorBy()
        {
            // Make sure there is an actor available to redo first!
            if(_lastMoveActor != null)
            {
                return _lastMoveActor.MoveBy(_lastActorPoint);
            }
            else
            {
                return false;
            }
        }

        // Undo last actor move
        // then clear the undo so it cannot be repeated
        public bool UndoMoveActorBy()
        {
            // Make sure there is an actor available to undo first!
            if(_lastMoveActor != null)
            {
                // reverse the directions of the last move
                _lastActorPoint = new Point(-_lastActorPoint.X, -_lastActorPoint.Y);

                if(_lastMoveActor.MoveBy(_lastActorPoint))
                {
                    _lastActorPoint = new Point(0, 0);
                    return true;
                }
                else
                {
                    _lastActorPoint = new Point(0, 0);
                    return false;
                }
            }
            return false;
        }

        // Tries to pick up an Item and add it to the Actor's
        // inventory list
        public void PickUp(Actor actor, Item item)
        {
            // Add the item to the Actor's inventory list
            // and then destroy it
            actor.Inventory.Add(item);
            item.Destroy();
            ColoredString message = new ColoredString($"{actor.Name} picked up {item.Name}");
            GameLoop.UIManager.MessageLog.Add(message);
        }

        // Executes an attack from an attacking actor
        // on a defending actor, and then describes
        // the outcome of the attack in the Message Log
        public void Attack(Actor attacker, Actor defender)
        {
            // Create two messages that describe the outcome
            // of the attack and defense
            ColoredString attackMessage = new ColoredString("");
            ColoredString defenseMessage = new ColoredString("");

            // Count up the amount of attacking damage done
            // and the number of successful blocks
            int hits = ResolveAttack(attacker, defender, attackMessage);
            int blocks = ResolveDefense(defender, hits, attackMessage, defenseMessage);

            // Display the outcome of the attack & defense
            GameLoop.UIManager.MessageLog.Add(attackMessage);
            if(!string.IsNullOrWhiteSpace(defenseMessage.ToString()))
            {
                GameLoop.UIManager.MessageLog.Add(defenseMessage);
            }

            int damage = hits - blocks;
            // The defender now takes damage

            ResolveDamage(defender, damage);
        }

        // Calculates the outcome of an attacker's attempt
        // at scoring a hit on a defender, using the attacker's
        // AttackChance and a random d100 roll as the basis.
        // Modifies a StringBuilder message that will be displayed
        // in the MessageLog
        private static int ResolveAttack(Actor attacker, Actor defender, ColoredString attackMessage)
        {
            // Create a string that expresses the attacker and defender's names
            int hits = 0;
            attackMessage.String += ($"{attacker.Name} attacks {defender.Name}, ");

            // The attacker's Attack value determines the number of D100 dice rolled
            for(int i = 0; i < attacker.Attack; i++)
            {
                int diceOutcome = Dice.Roll("1d100");

                //Resolve the dicing outcome and register a hit, governed by the
                //attacker's AttackChance value.
                if(diceOutcome >= 100 - attacker.AttackChance)
                {
                    hits++;
                }
            }
            return hits;
        }

        // Calculates the outcome of a defender's attempt
        // at blocking incoming hits.
        // Modifies a StringBuilder messages that will be displayed
        // in the MessageLog, expressing the number of hits blocked.
        private static int ResolveDefense(Actor defender, int hits, ColoredString attackMessage, ColoredString defenseMessage)
        {
            int blocks = 0;
            if (hits > 0)
            {
                // Create a string that displays the defender's name and outcomes
                attackMessage.String += ($"scoring {hits} hits.");
                defenseMessage.String += ($"{defender.Name} defends and rolls ");

                //The defender's Defense value determines the number of D100 dice rolled
                for (int i = 0; i < defender.Defense; i++)
                {
                    //Roll a single D100 and add its results to the defense Message
                    int diceOutcome = Dice.Roll("1d100");

                    //Resolve the dicing outcome and register a block, governed by the
                    //attacker's DefenceChance value.
                    if (diceOutcome >= 100 - defender.DefenseChance)
                    {
                        blocks++;
                    }
                }
                defenseMessage.String += ($"resulting in {blocks} blocks.");
            }
            else
            {
                attackMessage.String += ("and misses completely!;");
            }
            return blocks;
        }

        // Calculates the damage a defender takes after a successful hit
        // and subtracts it from its Health
        // Then displays the outcome in the MessageLog.
        private static void ResolveDamage(Actor defender, int damage)
        {
            if(damage > 0)
            {
                defender.Health -= damage;
                ColoredString message = new ColoredString($" {defender.Name} was hit for {damage} damage");
                GameLoop.UIManager.MessageLog.Add(message);

                if(defender.Health <= 0)
                {
                    ResolveDeath(defender);
                }
            }
            else
            {
                ColoredString message = new ColoredString($"{defender.Name} blocked all damage!");
                GameLoop.UIManager.MessageLog.Add(message);
            }
        }

        // Removes an Actor that has died
        // and displays a message showing
        // the actor that has died, and they loot they dropped
        private static void ResolveDeath(Actor actor)
        {
            // Set up a customized death message
            ColoredString deathMessage = new ColoredString($"{actor.Name} has died");

            // dump the dead actor's inventory (if any)
            // at the map position where it died
            if(actor.Inventory.Count > 0)
            {
                deathMessage.String += (" and dropped");

                foreach (Item item in actor.Inventory)
                {
                    // move the Item to the place where the actor died
                    item.Position = actor.Position;

                    // Now let the MultiSpatialMap know that the Item is visible
                    
                    GameLoop.World.CurrentMap.Add(item);
                    
                    // Append the item to the deathMessage
                    deathMessage.String += (", " + item.Name);
                }

                // Clear the actor's inventory. Not strictly
                // necessary, but makes for good coding habits!
                actor.Inventory.Clear();
            }
            else
            {
                // The monster carries no loot, so don't show any loot dropped
                deathMessage.String += (".");
            }

            // actor goes bye-bye
            GameLoop.World.CurrentMap.Remove(actor);

            // Now show the deathMessage in the messagelog
            GameLoop.UIManager.MessageLog.Add(deathMessage);


            if (actor is Player)
            {
                ColoredString message = new ColoredString($" {actor.Name} was killed.");
                GameLoop.UIManager.MessageLog.Add(message);
            }
            else if(actor is Monster)
            {
                ColoredString message = new ColoredString($"{actor.Name} died and dropped {actor.Gold} gold coins.");
                GameLoop.UIManager.MessageLog.Add(message);

                //Update the player stats and call the refresh function to draw them
                GameLoop.World.Player.Gold += actor.Gold;
                
                var stat = new ColoredString($"Gold: {GameLoop.World.Player.Gold}", Color.Gold, Color.Transparent);
                GameLoop.UIManager.StatWindow._lines[5] = stat;
                GameLoop.UIManager.StatWindow.Refresh();
            }
        }
    }
}
