using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.XPath;
using Day13_MineCartMadness.Carts;
using Day13_MineCartMadness.Navigation;
using Day13_MineCartMadness.Rails;
using Day13_MineCartMadness.Tracks;
using Console = Colorful.Console;

namespace Day13_MineCartMadness
{
    public class TrackGrid
    {
        public TrackGrid(string inputPath)
        {
            Tracks = new List<Track>();
            AllCarts = new List<Cart>();
            IntersectionMap = new Dictionary<Coord, Intersection>();
            initGrid(inputPath);
        }

        public List<Track> Tracks { get; }
        public List<Cart> AllCarts { get; set; }
        public Dictionary<Coord, Intersection> IntersectionMap { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }

        private int dumpConsoleLeft = -1;
        private int dumpConsoleTop = -1;
        public void DumpGrid()
        {
            var consoleLeftTemp = -1;
            var consoleTopTemp = -1;
            if (dumpConsoleLeft == -1)
            {
                dumpConsoleLeft = Console.CursorLeft;
                dumpConsoleTop = Console.CursorTop;
            }
            else
            {
                consoleLeftTemp = Console.CursorLeft;
                consoleTopTemp = Console.CursorTop;
                Console.SetCursorPosition(dumpConsoleLeft, dumpConsoleTop);
            }

            var dumpGrid = new char[MaxY][];
            for (var i = 0; i < MaxY; i++) dumpGrid[i] = new char[MaxX];

            foreach (var t in Tracks)
            foreach (var r in t.Rails)
                dumpGrid[r.Coordinates.X][r.Coordinates.Y] = r.RailType;

            foreach (var c in AllCarts) dumpGrid[c.Coordinates.X][c.Coordinates.Y] = (char) c.CurrentDirection;

            for (var x = 0; x < dumpGrid.Length; x++)
            {
                for (var y = 0; y < dumpGrid[x].Length; y++)
                {
                    var output = dumpGrid[x][y];
                    if(output.IsCart())
                        Console.Write(output, Color.Green);
                    else
                        Console.Write(dumpGrid[x][y]);
                }
                Console.WriteLine();
            }

            if (consoleLeftTemp != -1)
            {
                Console.SetCursorPosition(consoleLeftTemp, consoleTopTemp);
            }
        }

        private void initGrid(string path)
        {
            var input = File.ReadAllLines(path);
            var indexCounter = 0;
            var grid = File.ReadAllLines(path).Select(x => x.ToCharArray()).ToArray();
            initTracks(grid);
            foreach (var t in Tracks)
            foreach (var c in t.CartsOnTrack)
                AllCarts.Add(c);
        }

        private void DumpCarts()
        {
            foreach(var c in AllCarts.GroupBy(x => x.Coordinates.X).OrderBy(x => x.Key))
                foreach(var ca in c.OrderBy(x=>x.Coordinates.Y))
                    ca.WhereAmI();
        }
        public void StartMoving(int delay = 0, bool dumpGrid = false)
        {
            var tickCount = 0;
            var rootCoord = new Coord(0, 0);
            
            while (true)
            {
                var cartsToRemove = new List<Cart>();
                var cartsByRow = AllCarts.GroupBy(x => x.Coordinates.X).OrderBy(x=>x.Key);
                foreach (var g in cartsByRow)
                {
                    foreach (var c in g.OrderBy(x=>x.Coordinates.Y))
                    {
                        if (cartsToRemove.Contains(c))
                            continue;
                        var result = c.Move(IntersectionMap);
                        if (!result.Deleted && !result.Success)
                            throw new InvalidOperationException("This should never happen");
                        //if (result.Deleted)
                        //{
                        //    Console.WriteLine($"{result.DeletedCartA.CurrentDirection} crashed into {result.DeletedCartB.CurrentDirection} at {result.DeletedCartA.Coordinates.Y}, {result.DeletedCartA.Coordinates.X} on tick {tickCount+1}. {AllCarts.Count-2} carts left.");
                        //    result.DeletedCartA.Destroy();
                        //    result.DeletedCartB.Destroy();
                            
                        //    cartsToRemove.Add(result.DeletedCartA);
                        //    cartsToRemove.Add(result.DeletedCartB);
                        //}

                        var collidingCarts = AllCarts.Where(x => x.Coordinates.Equals(c.Coordinates)).ToList();
                        if (collidingCarts.Count() > 1)
                        {
                            Console.WriteLine($"{collidingCarts[0].CurrentDirection} " +
                                              $"crashed into {collidingCarts[1].CurrentDirection} " +
                                              $"at {collidingCarts[0].Coordinates.Y}, {collidingCarts[0].Coordinates.X} " +
                                              $"on tick {tickCount + 1}. {AllCarts.Count - 2} carts left.");
                            cartsToRemove.AddRange(collidingCarts);
                        }
                    }
                }
                //foreach (var c in AllCarts.OrderBy(x => x.Coordinates.DistanceFrom(rootCoord)))
                //{
                //    if (cartsToRemove.Contains(c))
                //        continue;
                //    var result = c.Move(IntersectionMap);
                //    if(!result.Deleted && !result.Success)
                //        throw new InvalidOperationException("This should never happen");
                //    if (result.Deleted)
                //    {
                //        result.DeletedCartA.Destroy();
                //        result.DeletedCartB.Destroy();
                //        cartsToRemove.Add(result.DeletedCartA);
                //        cartsToRemove.Add(result.DeletedCartB);
                //    }
                //}

                foreach (var cartToRemove in cartsToRemove)
                    AllCarts.Remove(cartToRemove);
                //foreach(var t in Tracks)
                //foreach (var c in t.CartsOnTrack)
                //    c.Move(IntersectionMap);
                foreach (var t in Tracks)
                {
                    t.CartsOnTrack.Clear();
                    var newCarts = AllCarts.Where(x => x.OnTrack.TrackId == t.TrackId).ToList();
                    t.CartsOnTrack.AddRange(newCarts);
                }

                if(dumpGrid)
                    DumpGrid();
                foreach (var c in AllCarts)
                    c.ResetMovement();

                tickCount++;
                if (AllCarts.Count <= 1)
                    break;
                if (delay > 0)
                    Thread.Sleep(delay);
            }

            if (AllCarts.Count == 0)
            {
                Console.WriteLine("No Carts left! Finished Moving!");
            }
            else
            {
                Console.WriteLine($"{AllCarts[0].Coordinates.Y}, {AllCarts[0].Coordinates.X}");
            }
        }

        private void initTracks(char[][] grid)
        {
            //find next top left
            MaxY = grid.Length;
            for (var x = 0; x < grid.Length; x++)
            {
                var newMaxX = 0;
                for (var y = 0; y < grid[x].Length; y++)
                {
                    var currRail = grid[x][y];
                    //if currRail is a top left rail of a track, initialize that track
                    if (currRail.IsCurve() && x + 1 < grid.Length && y + 1 < grid[x].Length &&
                        currRail.IsTopLeftCurve(grid[x + 1][y], grid[x][y + 1]))
                        initTrack(x, y, grid);
                    newMaxX++;
                }

                if (newMaxX > MaxX)
                    MaxX = newMaxX;
            }
        }

        private void initTrack(int startX, int startY, char[][] grid)
        {
            //Place a "ghost" cart on the track that will ride the entire length of the track, acting as a "rail layer" so-to-speak
            //since we are starting at the top left, we can safely go right OR down. But we are gonna choose right.
            var track = new Track(new Coord(startX, startY));
            track.AddRail(startX, startY, grid[startX][startY], true);

            //Now, starting from our initial position, "ride" along the track, "laying" the rails and turning as needed until we get back
            //to our initial position.
            var nextX = startX;
            var nextY = startY + 1;
            var currentDirection = Direction.Right;
            //In our array of arrays, "Down" is when X is INCREASING. "Right" is when Y is INCREASING.
            while (!track.IsComplete)
            {
                var nextRail = grid[nextX][nextY];
                if (!nextRail.IsCurve())
                {
                    if (nextRail.IsIntersection())
                        track.AddRail(nextX, nextY, nextRail, IntersectionMap);
                    else
                        track.AddRail(nextX, nextY, nextRail);
                    switch (currentDirection)
                    {
                        case Direction.Right:
                            nextY++;
                            break;
                        case Direction.Left:
                            nextY--;
                            break;
                        case Direction.Down:
                            nextX++;
                            break;
                        case Direction.Up:
                            nextX--;
                            break;
                    }
                }
                else
                {
                    track.AddRail(nextX, nextY, nextRail);
                    if (nextRail == '\\')
                        switch (currentDirection)
                        {
                            case Direction.Right:
                                currentDirection = Direction.Down;
                                nextX = nextX + 1;
                                break;
                            case Direction.Left:
                                currentDirection = Direction.Up;
                                nextX = nextX - 1;
                                break;
                            case Direction.Down:
                                currentDirection = Direction.Right;
                                nextY = nextY + 1;
                                break;
                            case Direction.Up:
                                currentDirection = Direction.Left;
                                nextY = nextY - 1;
                                Console.WriteLine("At bottom switch");
                                break;
                        }
                    else if (nextRail == '/')
                        switch (currentDirection)
                        {
                            case Direction.Down:
                                currentDirection = Direction.Left;
                                nextY = nextY - 1;
                                break;
                            case Direction.Up:
                                currentDirection = Direction.Right;
                                nextY = nextY + 1;
                                break;
                            case Direction.Right:
                                currentDirection = Direction.Up;
                                nextX = nextX - 1;
                                break;
                            case Direction.Left:
                                currentDirection = Direction.Down;
                                nextX = nextX + 1;
                                break;
                        }
                }
            }

            Tracks.Add(track);
        }
    }
}