using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype.Data.Models.VoteResults
{
    public class RecallResult : JudgeResult { }
    public class JudgeResult : PropResult
    {
        public int CandidateId { get; set; }

        public JudgeResult() { }
    }
}
