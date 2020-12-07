using System;
using System.Collections.Generic;
using System.Text;
using Prototype.Data;


namespace EngineServer.Message
{
    public class CandidateInfoRecord : CandidateInfo
    {
        public ContestId ContestId { get; set; }
    }
}
