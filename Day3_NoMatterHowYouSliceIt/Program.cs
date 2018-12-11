using System;
using System.Collections.Generic;
using System.IO;

namespace Day3_NoMatterHowYouSliceIt
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var rawClaims = File.ReadAllLines("input.txt");

            var allRectangleClaims = new List<ClaimRectangle>();
            foreach (var rawClaim in rawClaims)
            {
                var claimParams = rawClaim.Split(' ');
                var claimId = claimParams[0];
                var edgeLengths = claimParams[2].TrimEnd(':').Split(',');
                var dimensions = claimParams[3].Split('x');
                var newClaim =
                    new ClaimRectangle(edgeLengths[0], edgeLengths[1], claimId, dimensions[0], dimensions[1]);
                allRectangleClaims.Add(newClaim);
            }

            //Now we have all of our claim data out of the input file, so lets begin adding it to our fabric
            var mainFabric = new Fabric(1000, 1000);
            foreach (var rect in allRectangleClaims) mainFabric.AddClaim(rect);

            var overlap = mainFabric.Overlap();
            mainFabric.FindNoOverlap();
            Console.ReadLine();
        }
    }
}