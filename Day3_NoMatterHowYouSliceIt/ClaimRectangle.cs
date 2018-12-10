using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day3_NoMatterHowYouSliceIt
{
    public class ClaimRectangle
    {
        public int LeftEdgeLength { get; set; }
        public int TopEdgeLength { get; set; }
        public string ID { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }

        public ClaimRectangle(int leftEdgeLength, int topEdgeLength, string id, int width, int length)
        {
            LeftEdgeLength = leftEdgeLength;
            TopEdgeLength = topEdgeLength;
            ID = id;
            Width = width;
            Length = length;
        }

        public ClaimRectangle(string leftEdgeLength, string topEdgeLength, string id, string width, string length)
        {
            LeftEdgeLength = Convert.ToInt32(leftEdgeLength);
            TopEdgeLength = Convert.ToInt32(topEdgeLength);
            ID = id;
            Width = Convert.ToInt32(width);
            Length = Convert.ToInt32(length);
        }

        public ClaimRectangle()
        {
        }
    }
}