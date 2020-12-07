using Prototype.Data.Models.VoteResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineServer.Message
{
    public class JudgeRecord : RecordBase
    {
        public List<JudgeResult> ResultRecords { get; set; } = new List<JudgeResult>();
        public JudgeRecord() { }
        public JudgeRecord(RecordBase r) : base(r) { }
    }

}
