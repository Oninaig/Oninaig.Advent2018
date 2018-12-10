using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day3_NoMatterHowYouSliceIt
{
    public class Fabric
    {

        public int Length { get; set; }
        public int Width { get;set; }

        //Our fabric grid
        public List<List<Claims>> Grid { get;set; }


        public Fabric(int length, int width)
        {
            Length = length;
            Width = width;
            this.Grid = new List<List<Claims>>(Length);
            for (int i = 0; i < Length; i++)
            {
                var clmList = new List<Claims>(Width);
                for (int j = 0; j < Width; j++)
                {
                    clmList.Add(new Claims());
                }
                Grid.Add(clmList);
            }
        }

        public void AddClaim(ClaimRectangle claim)
        {
            var leftStart = claim.LeftEdgeLength;
            var topStart = claim.TopEdgeLength;

            for (int i = 0; i < claim.Width; i++)
            {
                for (int j = 0; j < claim.Length; j++)
                {
                    Grid[leftStart + i][topStart + j].AddClaim(claim.ID);
                }
            }
        }

        public int Overlap()
        {
            var overlap = 0;
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    if (Grid[i][j].ClaimIds.Count() > 1)
                        overlap++;
                }
            }

            return overlap;
        }

        public Fabric() : this(1000,1000)
        {
        }
    }
}
