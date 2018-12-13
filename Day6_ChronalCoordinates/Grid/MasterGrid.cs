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

            //Need to transpose this
            //for (int i = 0; i < GridData.GetLength(0); i++)
            //{
            //    for (int j = 0; j < GridData[i].Length; j++)
            //    {
            //        var coordData = GridData[i][j];


            //        Console.Write($"{GridData[coordData.Coords.x][coordData.Coords.y].Data} ");
            //    }

            //    Console.WriteLine();
            //}

            //Traditional view where 0,0 is bottom left.
            //for (int i = GridData.GetLength(0) - 1; i>=0; i--)
            //{
            //    for (int j = 0; j < GridData[i].Length; j++)
            //    {
            //        Console.Write($"{GridData[i][j].Data} ");
            //    }

            //    Console.WriteLine();
            //}
        }
        public MasterGrid(){}

        public MasterGrid(int maxX, int maxY, int minX, int minY)
        {
            this.MetaData = new MasterGridMeta(maxX, maxY, minX, minY);
            labelEnumerator = Constants.AlphabetList.GetEnumerator();
            labelEnumerator.MoveNext();
        }

        public int MaxArea()
        {
            return (MetaData.MaxX - MetaData.MinX) * (MetaData.MaxY - MetaData.MinY);
        }

        public void SetCoordinateData(Coordinate coord, object data)
        {
            GridData[coord.x + MetaData.RelativeOffsetX][coord.y + MetaData.RelativeOffsetY] =
                new CoordinateData(data, coord);
        }

        public void InitGridData(IEnumerable<Coordinate> seedCoords)
        {
            GridData = new CoordinateData[GridArrLength][];
            for (int i = 0; i < GridArrLength; i++)
            {
                GridData[i] = new CoordinateData[GridArrWidth];
                for (int j = 0; j < GridArrWidth; j++)
                {
                    GridData[i][j] = new CoordinateData($"A{Constants.EmptyCoord}", new Coordinate(i - MetaData.RelativeOffsetX, j - MetaData.RelativeOffsetY));
                }
            }

            if (seedCoords != null)
            {
                var arr = seedCoords.ToArray();
                //Risky to use just ints for our data because our "empty" coordinate uses a period char, which is equivalent to 46 when expressed as an int. 
                //Lets change this to use letters intead.
                for (int i = 0; i < arr.Length; i++)
                {
                    SetCoordinateData(arr[i], labelEnumerator.Current);
                    labelEnumerator.MoveNext();
                }
            }
        }

        public CoordinateData RetrieveCoordinateData(int coordX, int coordY)
        {
            return GridData[coordX + MetaData.RelativeOffsetX][coordY + MetaData.RelativeOffsetY];
        }
        public CoordinateData RetrieveCoordinateData(Coordinate coord)
        {
            return GridData[coord.x + MetaData.RelativeOffsetX][coord.y + MetaData.RelativeOffsetY];
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
                    var closestCoord = new Coordinate(int.MaxValue, int.MaxValue);
                    bool tied = false;
                    foreach (var coord in seedCoords)
                    {
                        //var label = $"A{++counter}";
                        //if (currCoordData.Coords.Equals(coord))
                        //{
                        //    SetCoordinateData(coord, label);
                        //    continue;
                        //}

                        var currDistance = currCoordData.Coords.ManhanttanDistance(coord);
                        if (currDistance == shortestDistance && shortestDistance != -1)
                            tied = true;

                        if (shortestDistance == -1 || currDistance < shortestDistance)
                        {
                            
                            shortestDistance = currDistance;
                            closestCoord = coord;
                        }
                    }

                    if (shortestDistance > 0 && closestCoord.x != int.MaxValue)
                    {
                        var closestLabel = RetrieveCoordinateData(closestCoord).Data;
                        if(tied)
                            SetCoordinateData(new Coordinate(i,j),$"{Constants.EmptyCoord}");
                        else
                            SetCoordinateData(new Coordinate(i,j),closestLabel.ToString().ToLower());
                    }

                }
            }
        }
    }
}
