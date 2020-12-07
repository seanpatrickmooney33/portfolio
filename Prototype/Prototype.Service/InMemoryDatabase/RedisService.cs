using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace Prototype.Service
{
    public class RedisService : IRedisService
    {
        private ConnectionMultiplexer Redis { get; set; }
        public RedisService(ConnectionMultiplexer r)
        {
            Redis = r;
        }

        public async Task<T> Get<T>(string key) where T : class
        {
            String value = await Redis.GetDatabase().StringGetAsync(key);
            if (String.IsNullOrWhiteSpace(value)) return null;
            return JsonSerializer.Deserialize<T>(value);
        }

        public void Write(string key, string Value)
        {
            throw new NotImplementedException();
        }

        public string Get(string key)
        {
            throw new NotImplementedException();
        }
    }
}
