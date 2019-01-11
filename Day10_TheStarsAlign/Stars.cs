using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Day10_TheStarsAlign
{
    public class Stars
    {
        
        public Dictionary<Point, List<Velocity>> StarCoordinates;
        public string[,] StarSystem;
        public int MaxX;
        /// <summary>
        /// Negative is UP
        /// </summary>
        public int MaxY;

        public int MinX;
        public int MinY;
        public Stars(string inputPath)
        {
            StarCoordinates = new Dictionary<Point, List<Velocity>>();
            var input = File.ReadAllLines(inputPath);
            foreach (var line in input)
            {
                var positionInput = Regex.Replace(line.Substring(9, 8), @"\s|<|>", "").Split(',')
                    .Select(x => Convert.ToInt32(x)).ToArray();
                var velocityInput = Regex.Replace(line.Substring(27, 8), @"\s|<|>", "").Split(',')
                    .Select(x => Convert.ToInt32(x)).ToArray();
                var newPoint = new Point(positionInput[0], positionInput[1], false);
                var newVelocity = new Velocity(velocityInput[0], velocityInput[1]);
                if (newPoint.X > MaxX)
                    MaxX = newPoint.X;
                if (newPoint.X < MinX)
                    MinX = newPoint.X;
                if (newPoint.Y < MaxY) // negative is UP
                    MaxY = newPoint.Y;
                if (newPoint.Y > MinY)
                    MinY = newPoint.Y;
                if (!StarCoordinates.ContainsKey(newPoint))
                    StarCoordinates[newPoint] = new List<Velocity>();
                StarCoordinates[newPoint].Add(newVelocity);
            }

            var width = Math.Abs(MaxX - MinX);
            var length = Math.Abs(MinY - MaxY);
            StarSystem = new string[width,length];
        }

        public void DumpStarSystem()
        {
            for (int i = 0; i < StarSystem.GetLength(1)+1; i++)
            {
                for (int j = 0; j < StarSystem.GetLength(0)+1; j++)
                {
                    var indexAsPoint = new Point(j+ MinX, i+ MaxY, true);
                    if (StarCoordinates.ContainsKey(indexAsPoint))
                        Console.Write("# ");
                    else
                        Console.Write(". ");
                }
                Console.WriteLine();
            }
        }

        public void TimeStep()
        {
            //var toDelete = new List<Point>();
            var toAdd = new Dictionary<Point, List<Velocity>>();
            var toAddBuffer = new List<Point>();
            var tooAddDupes = new List<Point>();
            Console.WriteLine("Press any key to perform a timestep. Type 'q' to quit");
            while (Console.ReadLine().Trim() != "q")
            {
                foreach (var kvp in StarCoordinates)
                {
                    var currPoint = kvp.Key;
                    foreach (var currVelocity in kvp.Value)
                    {

                        var newX = currPoint.X + currVelocity.X;

                        var newY = currPoint.Y + currVelocity.Y;

                        var newPoint = new Point(newX, newY, false);
                        if (!toAdd.ContainsKey(newPoint))
                            toAdd[newPoint] = new List<Velocity>();

                        toAdd[newPoint].Add(currVelocity);
                    }
                    
                    
                }
                StarCoordinates = new Dictionary<Point, List<Velocity>>(toAdd);
                toAdd.Clear();
                tooAddDupes.Clear();
                toAddBuffer.Clear();
                DumpStarSystem();
                Console.WriteLine("Press any key to perform another timestep. Type 'q' to quit");
            }
        }
    }


    public struct Point : IEquatable<Point>
    {
        /// <summary>
        /// Left is NEGATIVE
        /// Right is POSITIVE
        /// </summary>
        public int X;
        /// <summary>
        /// Up is NEGATIVE
        /// Down is POSITIVE
        /// </summary>
        public int Y;

        public bool Empty;
        
        public Point(int x, int y, bool empty)
        {
            X = x;
            Y = y;
            Empty = empty;
        }
        public bool Equals(Point other)
        {
            if (this.X == other.X && this.Y == other.Y)
                return true;
            return false;
        }
    }
    public struct Velocity
    {
        
        public int X;
        
        public int Y;

        public Velocity(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    
}
