using System;
using System.Collections.Generic;
using System.Linq;

namespace Day3_NoMatterHowYouSliceIt
{
    public class Fabric
    {
        public Fabric(int length, int width)
        {
            Length = length;
            Width = width;
            AllClaimIds = new List<string>();
            Grid = new List<List<Claims>>(Length);
            for (var i = 0; i < Length; i++)
            {
                var clmList = new List<Claims>(Width);
                for (var j = 0; j < Width; j++) clmList.Add(new Claims());
                Grid.Add(clmList);
            }
        }

        public Fabric() : this(1000, 1000)
        {
        }

        public int Length { get; set; }
        public int Width { get; set; }

        //Our fabric grid
        public List<List<Claims>> Grid { get; set; }

        //Our list of claimIds
        public List<string> AllClaimIds { get; set; }


        public void AddClaim(ClaimRectangle claim)
        {
            var leftStart = claim.LeftEdgeLength;
            var topStart = claim.TopEdgeLength;

            for (var i = 0; i < claim.Width; i++)
            for (var j = 0; j < claim.Length; j++)
                Grid[leftStart + i][topStart + j].AddClaim(claim.ID);

            if (!AllClaimIds.Contains(claim.ID))
                AllClaimIds.Add(claim.ID);
        }

        public int Overlap()
        {
            var overlap = 0;
            for (var i = 0; i < Width; i++)
            for (var j = 0; j < Length; j++)
                if (Grid[i][j].ClaimIds.Count() > 1)
                    overlap++;

            return overlap;
        }


        public void FindNoOverlap()
        {
            //We are going to find the claim with zero overlap by initially saying that everyone satisfies this condition, and then we are going to loop through the grid and whenever we find a claim that 
            //has more than 1 ID, we remove those IDs from the zero overlap list until we finish processing the grid. The result will be a list of IDs that have zero overlap.
            var tempClaimList = new List<string>(AllClaimIds);
            for (var i = 0; i < Width; i++)
            for (var j = 0; j < Length; j++)
            {
                var claimIds = Grid[i][j].ClaimIds;
                if (claimIds.Count() > 1)
                    foreach (var cId in claimIds)
                        tempClaimList.Remove(cId);
            }

            Console.WriteLine($"Claims that don't overlap at all: {tempClaimList}");
        }
    }
}