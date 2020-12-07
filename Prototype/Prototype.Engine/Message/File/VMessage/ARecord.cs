using System;
using System.Collections.Generic;
using System.Text;
using Prototype.Data;

namespace EngineServer.Message
{
    public class ARecord
    {
        public CountyTypeEnum County { get; set; }
        public ReportingTypeEnum ReportType { get; set; }
        public int ReportNumber { get; set; }
        public int CorrectionReportNumber { get; set; }
        public int PrecinctReporting { get; set; }
        public int TotalNumberOfPrecinct { get; set; }
        public double PrecinctReportingPercentage { get; set; }
        public int VoterTurnout { get; set; }
        public int TotalVoters { get; set; }
        public double VoterTurnoutPercentage { get; set; }
        public DateTime FirstReportDateTime { get; set; }
        public DateTime LastReportDateTime { get; set; }

    }
}
