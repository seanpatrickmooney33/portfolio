using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngineServer.Service
{
    public class RedisBuffer : IRedisBuffer
    {
        private static ConcurrentDictionary<String, String> Buffer = null;

        public RedisBuffer(int initialCapacity = 5000)
        {
            int concurrencyLevel = Environment.ProcessorCount * 2;
            if (RedisBuffer.Buffer == null)
            {
                RedisBuffer.Buffer = new ConcurrentDictionary<String, String>(concurrencyLevel, initialCapacity);
            }
        }

        public ConcurrentDictionary<String, String> GetBuffer()
        {
            return RedisBuffer.Buffer;
        }
    }
}
