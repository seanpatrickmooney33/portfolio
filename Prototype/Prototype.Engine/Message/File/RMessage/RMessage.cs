using System;
using System.Collections.Generic;
using System.Text;
using Prototype.Data;


namespace EngineServer.Message
{
    public class RaceMessage
    {
        public DateTime DateTime;
        public List<RaceRecord> rRecords = new List<RaceRecord>();
    }

    public class RaceRecord
    {
        public ContestId ContestId { get; set; }
        public String RaceName;
        // this data shoudl be part of the ElectionType enum but I have not identified all the data yet.
        public string ElectionShortName;
    }
}
