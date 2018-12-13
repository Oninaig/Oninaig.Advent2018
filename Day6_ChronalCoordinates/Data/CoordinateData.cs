using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day6_ChronalCoordinates.Data
{
    public struct CoordinateData
    {
        public object Data { get; set; }
        public Coordinate Coords { get;set; }
        public int DistanceToAllOthers { get; set; }

        public CoordinateData(object data, Coordinate coords, int distanceToAllOthers)
        {
            this.Data = data;
            this.Coords = coords;
            this.DistanceToAllOthers = distanceToAllOthers;
        }
    }
}
