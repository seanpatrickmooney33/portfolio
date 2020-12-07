using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Service
{
    public interface IRedisService
    {
        public Task<T> Get<T>(string key) where T : class;
        public String Get(String key);
        public void Write(String key, String Value);
    }
}
