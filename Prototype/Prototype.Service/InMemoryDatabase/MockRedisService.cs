using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.Service.InMemoryDatabase
{
    public class MockRedisService : IRedisService
    {
        private ConcurrentDictionary<String, String> Buffer { get; }

        public MockRedisService(int initialCapacity = 5000)
        {
            int concurrencyLevel = Environment.ProcessorCount * 2;
            this.Buffer = new ConcurrentDictionary<String, String>(concurrencyLevel, initialCapacity);
        }
        public string Get(string key)
        {
            if (Buffer.TryGetValue(key, out string value)) { return value; }
            return null;
        }

        public void Write(string key, string value)
        {
            Buffer.AddOrUpdate(key, value, (key, oldvalue) => { return value; });
        }

        public Task<T> Get<T>(string key) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
