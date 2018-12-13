using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Day6_ChronalCoordinates.Data;

namespace Day6_ChronalCoordinates.Grid
{
    public class MasterGrid
    {
        public MasterGridMeta MetaData { get; private set;}
        public CoordinateData[][] GridData { get; private set; }
        public int GridArrLength => MetaData.MaxX + 1;
        public int GridArrWidth => MetaData.MaxY + 1;

        public Dictionary<CoordinateData, int> CoordAreas { get; private set; }

        private IEnumerator labelEnumerator;

        public void PrintGrid()
        {

            for (int j = 0; j < GridData[0].Length; j++)
            {
                for (int i = 0; i < GridData.GetLength(0); i++)
                {
                    Console.Write($"{GridData[i][j].Data} ");
                }

                Console.WriteLine();
            }

        }
        public MasterGrid(){}

        
        public MasterGrid(int maxX, int maxY, int minX, int minY)
        {
            this.MetaData = new MasterGridMeta(maxX, maxY, minX, minY);
            labelEnumerator = Constants.AlphabetList.GetEnumerator();
            labelEnumerator.MoveNext();
            this.CoordAreas = new Dictionary<CoordinateData, int>();
        }


        public int MaxArea()
        {
            return (MetaData.MaxX - MetaData.MinX) * (MetaData.MaxY - MetaData.MinY);
        }

        public CoordinateData SetCoordinateData(Coordinate coord, object data, int distanceToAllOthers)
        {
            var newData = new CoordinateData(data, coord, distanceToAllOthers);
            GridData[coord.x + MetaData.RelativeOffsetX][coord.y + MetaData.RelativeOffsetY] = newData;
            return newData;
        }

        public void InitGridData(IEnumerable<Coordinate> seedCoords)
        {
            GridData = new CoordinateData[GridArrLength][];
            for (int i = 0; i < GridArrLength; i++)
            {
                GridData[i] = new CoordinateData[GridArrWidth];
                for (int j = 0; j < GridArrWidth; j++)
                {
                    GridData[i][j] = new CoordinateData($"{Constants.EmptyCoord}", new Coordinate(i - MetaData.RelativeOffsetX, j - MetaData.RelativeOffsetY), -1);
                }
            }

            if (seedCoords != null)
            {
                var arr = seedCoords.ToArray();
                //Risky to use just ints for our data because our "empty" coordinate uses a period char, which is equivalent to 46 when expressed as an int. 
                //Lets change this to use letters intead.
                var resetCount = 0;
                for (int i = 0; i < arr.Length; i++)
                {
                    var added = SetCoordinateData(arr[i], resetCount > 0 ? $"{labelEnumerator.Current}{resetCount}" : labelEnumerator.Current, -1);

                    if (!labelEnumerator.MoveNext())
                    {
                        resetCount++;
                        labelEnumerator.Reset();
                        labelEnumerator.MoveNext();
                    }
                    CoordAreas.Add(added, -1);
                }
            }
        }


        public bool IsInfinite(CoordinateData cData)
        {
            var currX = cData.Coords.x;
            var currY = cData.Coords.y;

            //Check left
            for (int i = currX; i >= 0; i--)
            {
                if (GridData[i][currY].Data.ToString().ToUpper() != cData.Data.ToString().ToUpper())
                    break;
                if (i == 0)
                    return true;
            }

            //check right
            for (int i = currX; i < GridData.GetLength(0); i++)
            {
                if (GridData[i][currY].Data.ToString().ToUpper() != cData.Data.ToString().ToUpper())
                    break;
                if (i == GridData.GetLength(0) - 1)
                    return true;
            }

            //check up
            for (int i = currY; i >=0 ; i--)
            {
                if (GridData[currX][i].Data.ToString().ToUpper() != cData.Data.ToString().ToUpper())
                    break;
                if (i == 0)
                    return true;
            }

            //check down
            for (int i = currY; i < GridData[0].Length ; i++)
            {
                if (GridData[currX][i].Data.ToString().ToUpper() != cData.Data.ToString().ToUpper())
                    break;
                if (i == GridData[0].Length - 1)
                    return true;
            }

            return false;
        }

        public void FindAreas()
        {
            var largestArea = 0;
            string largestAreaLabel = "XXXX";
            foreach (var kvp in CoordAreas)
            {
                //We are going to definte "infinite" as whether or not there is a way for a coordinate to reach the "edge" of the array without hitting
                //either an equidistance point or another character (upper or lowercase).
                //Example: In our 2D array we can iterate in two dimensions usually shown as i and j. Both i and j can go forwards and backwards so in essence
                //we have a way to go up, down, left, and right in our array. If we find a coordinate at the current i,j position in the array that has an uninterrupted
                //path to the end/beginning of either dimension, we can say it is infinite.
                var coordData = kvp.Key;
                var isInfinite = IsInfinite(coordData);
                if (isInfinite)
                    Console.WriteLine($"{coordData.Data} is infinite");
                else
                {
                    var maxArea = GetMaxAreaOfCoord(coordData);
                    Console.WriteLine($"{coordData.Data} is FINITE with area of: {maxArea}");
                    if (maxArea > largestArea)
                    {
                        largestArea = maxArea;
                        largestAreaLabel = coordData.Data.ToString();
                    }
                        
                }

            }

            Console.WriteLine($"Largest FINITE area is coordinate {largestAreaLabel} with area {largestArea}.");
        }

        private int GetMaxAreaOfCoord(CoordinateData coordData)
        {
            var area = 0;
            for (int i = 0; i < GridArrLength; i++)
            {
                for (int j = 0; j < GridArrWidth; j++)
                {
                    var currCoordData = GridData[i][j];
                    if (currCoordData.Data.ToString().ToUpper() == coordData.Data.ToString().ToUpper())
                        area++;
                }
            }

            return area;
        }

        public CoordinateData RetrieveCoordinateData(int coordX, int coordY)
        {
            return GridData[coordX + MetaData.RelativeOffsetX][coordY + MetaData.RelativeOffsetY];
        }
        public CoordinateData RetrieveCoordinateData(Coordinate coord)
        {
            return GridData[coord.x + MetaData.RelativeOffsetX][coord.y + MetaData.RelativeOffsetY];
        }

        public void FindSafeRegion()
        {
            var regionArea = 0;
            for (int i = 0; i < GridArrLength; i++)
            {
                for (int j = 0; j < GridArrWidth; j++)
                {
                    var currCoordData = GridData[i][j];
                    if (currCoordData.DistanceToAllOthers < 10000)
                        regionArea++;
                }
            }

            Console.WriteLine($"Safe region area: {regionArea}");
        }
        public void FillGrid(IEnumerable<Coordinate> seedCoords)
        {
            var counter = 0;
            for (int i = 0; i < GridArrLength; i++)
            {
                for (int j = 0; j < GridArrWidth; j++)
                {
                    
                    var currCoordData = GridData[i][j];
                    
                    //we have our current coord, now we loop through our master/seed coords and calculate the manhattan distance between our
                    //current coord and the seed coord to find the closest one.
                    var shortestDistance = -1;
                    var totalDistance = 0;
                    var closestCoord = new Coordinate(int.MaxValue, int.MaxValue);
                    bool tied = false;
                    List<int> distances = new List<int>();
                    foreach (var coord in seedCoords)
                    {
                        var currDistance = currCoordData.Coords.ManhanttanDistance(coord);
                        totalDistance += currDistance;
                        distances.Add(currDistance);

                        if (shortestDistance == -1 || currDistance < shortestDistance)
                        {
                            
                            shortestDistance = currDistance;
                            closestCoord = coord;
                        }
                    }

                    if (distances.Count(x=>x==shortestDistance) > 1)
                        tied = true;

                    if (shortestDistance > 0 && closestCoord.x != int.MaxValue)
                    {
                        var closestLabel = RetrieveCoordinateData(closestCoord).Data;
                        if(tied)
                            SetCoordinateData(new Coordinate(i,j),$"{Constants.EmptyCoord}", totalDistance);
                        else
                            SetCoordinateData(new Coordinate(i,j),closestLabel.ToString().ToLower(),totalDistance);
                    }
                    else if (shortestDistance == 0)
                    {
                        SetCoordinateData(closestCoord, RetrieveCoordinateData(closestCoord).Data, totalDistance);
                        Console.WriteLine("We are at ourselves");
                    }

                }
            }
        }
    }
}
