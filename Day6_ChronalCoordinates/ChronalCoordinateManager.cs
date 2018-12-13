using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day6_ChronalCoordinates
{
    public class ChronalCoordinateManager
    {
        public IList<Coordinate> Coordinates { get; private set; }
        public Coordinate TopRight { get; private set; }
        public Coordinate BottomLeft { get; private set; }

        public struct MasterGrid
        {
            public int MaxX, MaxY, MinX, MinY;

            public MasterGrid(int maxX, int maxY, int minX, int minY)
            {
                MaxX = maxX;
                MaxY = maxY;
                MinX = minX;
                MinY = minY;
            }
        }

        public ChronalCoordinateManager()
        {
            this.Coordinates = new List<Coordinate>();
        }

        public void AddCoordinate(int x, int y)
        {
            Coordinates.Add(new Coordinate(x, y));
        }

        public void AddCoordinate(Coordinate coord)
        {
            Coordinates.Add(coord);
        }
        public void AddCoordinate(string x, string y)
        {
            if(!Int16.TryParse(x, out Int16 newX))
                throw new ArgumentException($"Cannot parse x value: {x}");
            if (!Int16.TryParse(y, out Int16 newY))
                throw new ArgumentException($"Cannot parse y value: {y}");
            Coordinates.Add(new Coordinate(newX, newY));
        }

        //Since the grid is "infinite", we need a way to add some sort of boundaries to our calculations.
        //We will use the highest and lowest x and y points to do this and draw a square around the largest possible area
        //in the entire grid. This master grid will contain all other coordinates since its corners are using the highest and lowest points
        //available.

        public void InitMasterGrid()
        {

            var maxX = Coordinates.Aggregate((agg, next) => next.x >= agg.x ? next : agg);
            var maxY = Coordinates.Aggregate((agg, next) => next.y >= agg.y ? next : agg);
            var minX = Coordinates.Aggregate((agg, next) => next.x <= agg.x ? next : agg);
            var minY = Coordinates.Aggregate((agg, next) => next.y <= agg.y ? next : agg);


            TopRight = new Coordinate(maxX.x, maxY.y);
            BottomLeft = new Coordinate(minX.x, minY.y);

            Console.WriteLine(
                $"Max X: {maxX} | Max Y: {maxY} | Min X: {minX} | Min Y: {minY}");
        }


    }
}
