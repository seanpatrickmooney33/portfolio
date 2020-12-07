using Prototype.Data.Models.VoteResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineServer.Message
{
    public class InputCandidateRecord : RecordBase
    {
        public List<CandidateRecord> ResultRecords { get; set; } = new List<CandidateRecord>();
        public InputCandidateRecord() { }
        public InputCandidateRecord(RecordBase r) : base(r) { }
    }

}
