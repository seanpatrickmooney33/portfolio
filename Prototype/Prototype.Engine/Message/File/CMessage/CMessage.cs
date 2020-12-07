using System;
using System.Collections.Generic;
using System.Text;

namespace EngineServer.Message
{
    public class CMessage
    {
        public DateTime DateTime;
        //public List<CandidateInfoRecord> candidateInfoRecords = new List<CandidateInfoRecord>();
        public Dictionary<int, CandidateInfoRecord> candidateInfoRecords = new Dictionary<int, CandidateInfoRecord>();
    }
}
