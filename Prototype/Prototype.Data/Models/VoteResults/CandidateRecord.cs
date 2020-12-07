using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype.Data.Models.VoteResults
{
    public class CandidateRecord
    {
        public int CandidateId { get; set; }
        public int Votes { get; set; }
        public double PartyPercent { get; set; }
        public double RacePercent { get; set; }

        public CandidateRecord() { }
    }
}
