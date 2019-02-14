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
using Day13_MineCartMadness.Navigation;
using Day13_MineCartMadness.Rails;
using Day13_MineCartMadness.Tracks;

namespace Day13_MineCartMadness
{
    public class TrackGrid
    {
        private List<Track> Tracks { get; set; }
        public Dictionary<Coord, Intersection> IntersectionMap { get; set; }
        public TrackGrid(string inputPath)
        {
            this.Tracks = new List<Track>();
            this.IntersectionMap = new Dictionary<Coord, Intersection>();
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
}
