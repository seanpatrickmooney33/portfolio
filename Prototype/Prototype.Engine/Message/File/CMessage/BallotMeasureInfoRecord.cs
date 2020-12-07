using Prototype.Data;
using Prototype.Data.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngineServer.Message.File.CMessage
{
    public class BallotMeasureInfoRecord : BallotMeasureInfo
    {
        public ContestId ContestId { get; set; }
    }
}
