using Prototype.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngineServer.Service
{
    public class Driver
    {
        private readonly IRedisBuffer RedisBuffer;
        private readonly IRedisService RedisService;
        public Driver(IRedisBuffer r, IRedisService s)
        {
            this.RedisBuffer = r;
            this.RedisService = s;
        }

        public void WriteToRedis()
        {
            RedisBuffer.GetBuffer().ToList().ForEach(pair => {
                RedisService.Write(pair.Key, pair.Value);
            });
        }
    }
}
