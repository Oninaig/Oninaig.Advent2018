using System;
using System.Collections.Generic;
using System.Linq;
using Day6_ChronalCoordinates.Data;
using Day6_ChronalCoordinates.Grid;

namespace Day6_ChronalCoordinates
{
    public class ChronalCoordinateManager
    {
        public ChronalCoordinateManager()
        {
            Coordinates = new List<Coordinate>();
            CoordinatesWithData = new List<CoordinateData>();
        }

        public IList<Coordinate> Coordinates { get; }
        public IList<CoordinateData> CoordinatesWithData { get; }

        public MasterGrid Grid { get; private set; }

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
            if (!short.TryParse(x, out var newX))
                throw new ArgumentException($"Cannot parse x value: {x}");
            if (!short.TryParse(y, out var newY))
                throw new ArgumentException($"Cannot parse y value: {y}");
            Coordinates.Add(new Coordinate(newX, newY));
        }

        //Since the grid is "infinite", we need a way to add some sort of boundaries to our calculations.
        //We will use the highest and lowest x and y points to do this and draw a square around the largest possible area
        //in the entire grid. This master grid will contain all other coordinates since its corners are using the highest and lowest points
        //available.
        public void InitMasterGrid(bool lazy = false)
        {
            var maxX = Coordinates.Aggregate((agg, next) => next.x >= agg.x ? next : agg);
            var maxY = Coordinates.Aggregate((agg, next) => next.y >= agg.y ? next : agg);
            var minX = Coordinates.Aggregate((agg, next) => next.x <= agg.x ? next : agg);
            var minY = Coordinates.Aggregate((agg, next) => next.y <= agg.y ? next : agg);

            Grid = new MasterGrid(maxX.x, maxY.y, minX.x, minY.y);
            Console.WriteLine(
                $"Max X: {maxX.x} {maxX} | Max Y: {maxY.y} {maxY} | Min X: {minX.x} {minX}| Min Y: {minY.y} {minY}");

            if (lazy)
                return;
            Grid.InitGridData(Coordinates);
            Grid.FillGrid(Coordinates);
        }

        public void PrintGrid()
        {
            Grid.PrintGrid();
        }

        public void FindAreas()
        {
            Grid.FindAreas();
        }

        public void FindSafeRegion()
        {
            Grid.FindSafeRegion();
        }
    }
}