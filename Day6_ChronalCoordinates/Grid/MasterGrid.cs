using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day6_ChronalCoordinates.Grid
{
    public class MasterGrid
    {
        public MasterGridMeta MetaData { get; private set;}

        public MasterGrid(){}

        public MasterGrid(int maxX, int maxY, int minX, int minY)
        {
            this.MetaData = new MasterGridMeta(maxX, maxY, minX, minY);
        }

        public int MaxArea()
        {
            return (MetaData.MaxX - MetaData.MinX) * (MetaData.MaxY - MetaData.MinY);
        }
    }
}
