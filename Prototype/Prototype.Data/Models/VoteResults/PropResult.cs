using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype.Data.Models.VoteResults
{
    public class PropResult
    {
        public int yesVotes { get; set; }
        public double yesPercent { get; set; }
        public int noVotes { get; set; }
        public double noPercent { get; set; }

        public PropResult() { }
    }
}
