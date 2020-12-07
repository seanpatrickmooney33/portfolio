using Prototype.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineServer.Message
{
    public class RecordBase
    {
        public ContestId ContestId { get; set; }
        public CountyTypeEnum County { get; set; }
        public int PrecinctReporting { get; set; }
        public int TotalNumberOfPrecinct { get; set; }
        public ReportingTypeEnum ReportType { get; set; }

        public RecordBase() { }
        public RecordBase(RecordBase r) {
            this.ContestId = r.ContestId;
            this.County = r.County;
            this.PrecinctReporting = r.PrecinctReporting;
            this.TotalNumberOfPrecinct = r.TotalNumberOfPrecinct;
            this.ReportType = r.ReportType;
        }
    }
}
