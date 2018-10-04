using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using LHGames.DataStructures;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LHGames
{
    internal class Bot
    {
        internal IPlayer PlayerInfo { get; set; }

        internal Bot() { }

        /// <summary>
        /// Gets called before ExecuteTurn. This is where you get your bot's state.
        /// </summary>
        /// <param name="playerInfo">Your bot's current state.</param>
        internal void BeforeTurn(IPlayer playerInfo)
        {
            PlayerInfo = playerInfo;
        }

        internal Point FindClosestResource(Map map)
        {
            PlayerInfo.Position.Visited = false;
            
            Queue<Point> q = new Queue<Point>();
            q.Enqueue(PlayerInfo.Position);
            while (q.Count > 0)
            {
                Point current = q.Dequeue();
                current.Visited = true;

                // We found a ressource, let's go!
                if (map.GetTileAt(current.X, current.Y) == TileContent.Resource)
                {
                    return current;
                }
                
                // Check left, right, up, down if not too far from player.
                Tile left = map.GetRealTileAt(current.X - 1, current.Y);
                if (left != null && Point.Distance(left.Position, PlayerInfo.Position) < map.VisibleDistance && left.TileType != TileContent.Wall)
                {
                    q.Enqueue(left.Position);
                }
                
                Tile right = map.GetRealTileAt(current.X + 1, current.Y);
                if (right != null && Point.Distance(right.Position, PlayerInfo.Position) < map.VisibleDistance && right.TileType != TileContent.Wall)
                {
                    q.Enqueue(right.Position);
                }
                
                Tile up = map.GetRealTileAt(current.X, current.Y + 1);
                if (up != null && Point.Distance(up.Position, PlayerInfo.Position) < map.VisibleDistance && up.TileType != TileContent.Wall)
                {
                    q.Enqueue(up.Position);
                }
                
                Tile down = map.GetRealTileAt(current.X, current.Y - 1);
                if (down != null && Point.Distance(down.Position, PlayerInfo.Position) < map.VisibleDistance && down.TileType != TileContent.Wall)
                {
                    q.Enqueue(down.Position);
                }
            }

            return null;
        }

        internal Point ResourceAround(Map map, Point point)
        {
            // Left
            if (map.GetTileAt(point.X - 1, point.Y) == TileContent.Resource)
            {
                return new Point(-1, 0);
            }
            
            // Right
            if (map.GetTileAt(point.X + 1, point.Y) == TileContent.Resource)
            {
                return new Point(1, 0);
            }
            
            // Up
            if (map.GetTileAt(point.X, point.Y + 1) == TileContent.Resource)
            {
                return new Point(0, 1);
            }
            
            // Down
            if (map.GetTileAt(point.X, point.Y - 1) == TileContent.Resource)
            {
                return new Point(0, -1);
            }
            
            return null;
        }

        internal string RandomMove()
        {
            Random random = new Random();
            // 0 is x-axis movement.
            if (random.Next(0, 2) == 0)
            {
                return AIHelper.CreateMoveAction(new Point(random.Next(0, 2) == 0 ? -1 : 1, 0));
            }
            
            // Else y-axis.
            return AIHelper.CreateMoveAction(new Point(0, random.Next(-1, 2) == 0 ? -1 : 1));
        }

        /// <summary>
        /// Implement your bot here.
        /// </summary>
        /// <param name="map">The gamemap.</param>
        /// <param name="visiblePlayers">Players that are visible to your bot.</param>
        /// <returns>The action you wish to execute.</returns>
        internal string ExecuteTurn(Map map, List<IPlayer> visiblePlayers)
        {
            // Full? Run!
            if (PlayerInfo.CarriedResources == PlayerInfo.CarryingCapacity)
            {
                int xDistance = PlayerInfo.HouseLocation.X - PlayerInfo.Position.X;
                int yDistance = PlayerInfo.HouseLocation.Y - PlayerInfo.Position.Y;
                
                // Prioritize highest distance.
                if (Math.Abs(xDistance) > Math.Abs(yDistance))
                {
                    Console.WriteLine("Moving to house in X.");
                    return AIHelper.CreateMoveAction(new Point(xDistance > 0 ? 1 : -1, 0));
                }
                
                Console.WriteLine("Moving to house in Y.");
                return AIHelper.CreateMoveAction(new Point(0, yDistance > 0 ? 1 : -1));
            }
            
            // Next to resource? Just mine.
            Point direction = ResourceAround(map, PlayerInfo.Position);
            if (direction != null)
            {
                Console.WriteLine("Collecting resource.");
                return AIHelper.CreateCollectAction(direction);
            }
            
            Point closest = FindClosestResource(map);
            // Go to resource. (doesn't avoid walls for now)
            if (closest != null)
            {
                int xDistance = closest.X - PlayerInfo.Position.X;
                int yDistance = closest.Y - PlayerInfo.Position.Y;
                
                // Prioritize highest distance.
                if (Math.Abs(xDistance) > Math.Abs(yDistance))
                {
                    Console.WriteLine("Moving to resource in X.");
                    return AIHelper.CreateMoveAction(new Point(xDistance > 0 ? 1 : -1, 0));
                }
                
                Console.WriteLine("Moving to resource in Y.");
                return AIHelper.CreateMoveAction(new Point(0, yDistance > 0 ? 1 : -1));
            }

            Console.WriteLine("Moving randomly.");
            // Couldn't find anything, randomly move to explore.
            return RandomMove();
        }

        /// <summary>
        /// Gets called after ExecuteTurn.
        /// </summary>
        internal void AfterTurn()
        {
        }
    }
}
