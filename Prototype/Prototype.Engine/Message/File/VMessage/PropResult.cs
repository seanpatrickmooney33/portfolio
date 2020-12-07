using Prototype.Data.Models.VoteResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineServer.Message
{
    public class RecallRecord : RecordBase
    {
        public List<RecallResult> ResultRecords { get; set; } = new List<RecallResult>();
        public RecallRecord() { }
        public RecallRecord(RecordBase r) : base(r) { }
    }

    public class PropRecord : RecordBase
    {
        public List<PropResult> ResultRecords { get; set; } = new List<PropResult>();
        public PropRecord() { }
        public PropRecord(RecordBase r) : base(r) { }
    }
}
