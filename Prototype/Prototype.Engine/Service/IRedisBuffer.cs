using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngineServer.Service
{
    public interface IRedisBuffer
    {
        ConcurrentDictionary<String, String> GetBuffer();
    }
}
