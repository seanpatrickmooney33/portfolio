using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngineServer.Message
{
    public interface IMessageProcessor
    {
        public void DoWork(String FileName, String FileContent);
    }
}
