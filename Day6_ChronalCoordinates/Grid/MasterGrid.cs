using System;
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

        public MasterGrid(){}

        public MasterGrid(int maxX, int maxY, int minX, int minY)
        {
            this.MetaData = new MasterGridMeta(maxX, maxY, minX, minY);
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
        public void InitGridData(IEnumerable<Coordinate> seedCoords = null)
        {
            GridData = new CoordinateData[MetaData.Length][];
            for (int i = 0; i < MetaData.Length; i++)
            {
                GridData[i] = new CoordinateData[MetaData.Width];
                for (int j = 0; j < MetaData.Width; j++)
                {
                    GridData[i][j] = new CoordinateData(Constants.EmptyCoord, new Coordinate(i - MetaData.RelativeOffsetX, j - MetaData.RelativeOffsetY));
                }
            }

            if (seedCoords != null)
            {
                var arr = seedCoords.ToArray();
                for (int i = 0; i < arr.Length; i++)
                    SetCoordinateData(arr[i], i);
            }
            
        }
    }
}
