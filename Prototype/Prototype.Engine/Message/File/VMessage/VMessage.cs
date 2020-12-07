using Prototype.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineServer.Message
{
    public class VMessage
    {
        public DateTime DateTime;
        public ElectionTypeEnum ElectionType;

        public List<InputCandidateRecord> CandidateResultRecords { get; set; } = new List<InputCandidateRecord>();
        public List<PropRecord> PropRecords { get; set; } = new List<PropRecord>();
        public List<JudgeRecord> JudgeRecords { get; set; } = new List<JudgeRecord>();
        public List<RecallRecord> RecallRecords { get; set; } = new List<RecallRecord>();
       
        public List<ARecord> aRecords { get; set; } = new List<ARecord>();

    }
}
