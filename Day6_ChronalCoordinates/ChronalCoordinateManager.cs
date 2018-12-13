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



    }
}
