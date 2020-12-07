using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype.Data.Models.VoteResults
{
    public class VoteResults<T>
    {
        public CandidateInfo CandidateInfo { get; set; }
        public T Result { get; set; }

        public VoteResults() { }
        public VoteResults(CandidateInfo i, T r)
        {
            CandidateInfo = i;
            Result = r;
        }
    }
}
