using System.Collections.Generic;

namespace Day3_NoMatterHowYouSliceIt
{
    public class Claims
    {
        public Claims(List<string> claimIds)
        {
            ClaimIds = claimIds;
        }

        public Claims()
        {
            ClaimIds = new List<string>();
        }

        public List<string> ClaimIds { get; set; }

        public void AddClaim(string ID)
        {
            ClaimIds.Add(ID);
        }
    }
}