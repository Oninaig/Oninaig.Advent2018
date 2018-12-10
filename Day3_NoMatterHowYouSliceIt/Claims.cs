using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day3_NoMatterHowYouSliceIt
{
    public class Claims
    {
        public List<string> ClaimIds { get; set; }

        public Claims(List<string> claimIds)
        {
            ClaimIds = claimIds;
        }

        public Claims()
        {
            this.ClaimIds = new List<string>();
        }

        public void AddClaim(string ID)
        {
            ClaimIds.Add(ID);
        }
    }
}
