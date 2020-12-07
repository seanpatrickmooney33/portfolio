using Portfolio.Common.Foundation;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Portfolio.Infastructure.Redis.Context
{
    public class RedisContext : IUnitOfWork
    {
        private readonly ConnectionMultiplexer _redis;
        public RedisContext(ConnectionMultiplexer r)
        {
            _redis = r;
        }
        public Task SaveEntitiesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
