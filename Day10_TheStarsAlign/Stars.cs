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
        //We are using a List of velocities because during a timestep, there can exist multiple points at the same location that have different velocities.
        //without a list, we would lose the "duplicate" points.
        public Dictionary<Point, List<Velocity>> StarCoordinates;
        public string[,] StarSystem;
        public int MaxX;
        /// <summary>
        /// Negative is UP
        /// </summary>
        public int MaxY;
        public int MinX;

        public int MinY;
        private int _totalPointCount;
        public Stars(string inputPath)
        {
            _totalPointCount = 0;
            StarCoordinates = new Dictionary<Point, List<Velocity>>();
            var input = File.ReadAllLines(inputPath);
            foreach (var line in input)
            {
                Regex posRegex = new Regex("<(-|\\s|\\d+)\\d*,\\s(-|\\s|\\d+)\\d*>");
                var matches = posRegex.Matches(line).Cast<Match>().Select(x=>Regex.Replace(x.Value, @"\s|<|>", "")).ToArray();
                var positionInput = matches[0].Split(',').Select(x => Convert.ToInt32(x)).ToArray();
                var velocityInput = matches[1].Split(',').Select(x => Convert.ToInt32(x)).ToArray();
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
                _totalPointCount++;
            }

            var width = Math.Abs(MaxX - MinX);
            var length = Math.Abs(MinY - MaxY);
           // StarSystem = new string[width,length];
        }

        public void DumpStarSystem()
        {
            if (StarSystem == null)
            {
                Console.WriteLine("Cannot dump grid. Grid not initialized yet.");
                return;
            }
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
            var toAdd = new Dictionary<Point, List<Velocity>>();
            Console.WriteLine("Press any key to perform a timestep. Type 'q' to quit");
            var currAverageDistance = double.MaxValue;
            var timeStepCounter = 0;
            while (true)
            {
                timeStepCounter++;
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
                if (currAverageDistance <= 20.0)
                {
                    currAverageDistance = averageDistance(Accuracy.High);
                    initStartSystem();
                }
                else
                    currAverageDistance = averageDistance(Accuracy.Low);
                Console.WriteLine(currAverageDistance);
                if (currAverageDistance <= 20.0)
                {
                    DumpStarSystem();
                    Console.WriteLine("Press enter to timestep (distance too close to auto-continue)");
                    Console.WriteLine($"Seconds elapsed: {timeStepCounter}");
                    Console.ReadLine();
                }
            }
        }

        private void initStartSystem()
        {
            MinX = StarCoordinates.OrderByDescending(x => x.Key.X).Last().Key.X;
            MaxX = StarCoordinates.OrderByDescending(x => x.Key.X).First().Key.X;
            MaxY= StarCoordinates.OrderByDescending(y => y.Key.Y).Last().Key.Y;
            MinY= StarCoordinates.OrderByDescending(y => y.Key.Y).First().Key.Y;
            var width = Math.Abs(MaxX - MinX);
            var length = Math.Abs(MinY - MaxY);
            StarSystem = new string[width, length];
        }

        private enum Accuracy
        {
            Low,
            High
        }
        private double averageDistance(Accuracy accuracy)
        {
            var totalDist = 0.0;

            if (accuracy == Accuracy.High)
            {
                foreach (var kvp in StarCoordinates)
                {
                    var accu = 0.0;
                    foreach (var kvp2 in StarCoordinates)
                    {
                        var dist = kvp.Key.DistanceFrom(kvp2.Key);
                        if (dist > 0.0)
                            accu += dist;
                    }

                    totalDist += (accu/(_totalPointCount-1.0));
                }

                return totalDist / _totalPointCount;
            }
            else
            {
                var accu = 0.0;
                var startPoint = StarCoordinates.First().Key;
                foreach (var kvp in StarCoordinates)
                {
                    var dist = kvp.Key.DistanceFrom(startPoint);
                    if (dist > 0.0)
                        accu += dist;
                }

                return accu / (double) _totalPointCount;
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

        public double DistanceFrom(Point otherPoint)
        {
            return Math.Sqrt(Math.Pow((otherPoint.X - X), 2) - Math.Pow((otherPoint.Y - Y), 2));
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
