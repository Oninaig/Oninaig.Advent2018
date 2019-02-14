using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Day13_MineCartMadness
{
    public static class MineCartExtensions
    {
        public static bool IsCart(this char c)
        {
            if (c == '<' || c == '>' || c == '^' || c == 'v')
                return true;
            return false;
        }

        public static bool IsCurve(this char c)
        {
            if (c == '/' || c == '\\')
                return true;
            return false;
        }

        public static bool IsTopLeftCurve(this char c)
        {
            if (c == '/')
                return true;
            return false;
        }

        public static bool IsIntersection(this char c)
        {
            if (c == '+')
                return true;
            return false;
        }

        public static bool IsHorizontalRail(this char c)
        {
            return (c == '-');
        }

        public static bool IsVerticalRail(this char c)
        {
            return c == '|';
        }

        /// <summary>
        /// Combines IsVerticalRail, IsCart, and IsIntersection (is c a vertical rail, a cart, or an intersection?
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsVertCartInter(this char c)
        {
            return c.IsVerticalRail() || c.IsCart() || c.IsIntersection();
        }

        /// <summary>
        /// Combines IsHorizontalRail, IsCart, and IsIntersection (is c a vertical rail, a cart, or an intersection?
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsHoriCartInter(this char c)
        {
            return c.IsHorizontalRail() || c.IsCart() || c.IsIntersection();
        }

        public static bool IsTopLeftCurve(this char c, char c1, char c2)
        {
            if (c.IsTopLeftCurve() && (c1.IsVertCartInter()) && (c2.IsHoriCartInter()))
                return true;
            return false;
        }

        public static CartDirection GetCartDirection(this char c)
        {
            return (CartDirection) c;
        }
    }


    
    public enum CartDirection
    {
        Up = '^',
        Down = 'v',
        Left = '<',
        Right = '>',
        Error = '#'
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum CartIntersectionBehavior
    {
        Left,
        Straight,
        Right
    }

    public class TrackGrid
    {
        private List<Track> Tracks { get; set; }
        public TrackGrid(string inputPath)
        {
            this.Tracks = new List<Track>();
            initGrid(inputPath);
        }
        private void initGrid(string path)
        {
            var input = File.ReadAllLines(path);
            var indexCounter = 0;
            var grid = File.ReadAllLines(path).Select(x => x.ToCharArray()).ToArray();
            initTracks(grid);
        }

        private void initTracks(char[][] grid)
        {
            //find next top left
            for (int x = 0; x < grid.Length; x++)
            {
                for (int y = 0; y < grid[x].Length; y++)
                {
                    var currRail = grid[x][y];
                    //if currRail is a top left rail of a track, initialize that track
                    if (currRail.IsCurve() && x+1 < grid.Length && y+1 < grid[x].Length && currRail.IsTopLeftCurve(grid[x+1][y], grid[x][y+1]))
                    {
                        initTrack(x, y, grid);
                    }
                    Console.Write(currRail);
                }

                Console.WriteLine();
            }
        }

        private void initTrack(int startX, int startY, char[][] grid)
        {
            //Place a "ghost" cart on the track that will ride the entire length of the track, acting as a "rail layer" so-to-speak
            //since we are starting at the top left, we can safely go right OR down. But we are gonna choose right.
            var track = new Track(new Coord(startX, startY));
            track.AddRail(startX, startY, grid[startX][startY]);

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
                    {
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
                        }
                    }
                    else if (nextRail == '/')
                    {
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
                        }
                    }
                }
            }
            Tracks.Add(track);
        }
    }

    public class Track
    {
        public Coord TopLeft { get; set; }
        public Guid TrackId { get; private set; }
        public LinkedList<Rail> Rails { get; private set; }
        public bool IsComplete { get; set; }
        public List<Cart> CartsOnTrack { get; private set; }
        public Track(Coord topleft)
        {
            this.TopLeft = topleft;
            this.TrackId = Guid.NewGuid();
            this.Rails = new LinkedList<Rail>();
            this.CartsOnTrack = new List<Cart>();
        }


        public void AddRail(int x, int y, char c)
        {
            if (c.IsCart())
            {
                CartsOnTrack.Add(new Cart(new Coord(x,y),this));
                switch (c.GetCartDirection())
                {
                    case CartDirection.Up:
                    case CartDirection.Down:
                        c = '|';
                        break;
                    case CartDirection.Left:
                    case CartDirection.Right:
                        c = '-';
                        break;
                }
            }

            Rails.AddLast(new Rail(this, c, new Coord(x, y)));
            //todo: this will fail to work if tracks are anything but squares/rectangles.
            if (TopLeft.X == x - 1 && TopLeft.Y == y)
                this.IsComplete = true;
        }
    }

    public class Cart
    {
        public Coord Coordinates { get; set; }
        public Track OnTrack { get; set; }

        public Cart(Coord coordinates, Track owner)
        {
            this.Coordinates = coordinates;
            this.OnTrack = owner;
        }
    }

    public class Rail
    {
        public char RailType { get; private set; }
        public Track OwnerTrack { get; private set; }
        public Coord Coordinates { get; private set; }
        public Rail(Track owner, char railType, Coord coordinates)
        {
            this.OwnerTrack = owner;
            this.RailType = railType;
            this.Coordinates = coordinates;
        }
    }

    public struct Coord
    {
        public int X, Y;

        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
