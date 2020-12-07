using Portfolio.Common.Foundation;
using Portfolio.Domain.AggregrateModels.RaceAggregateModel;
using Portfolio.Infastructure.Redis.Context;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portfolio.Infastructure.Redis.Repositories
{
    public class RaceRepository : IRaceRepository
    {
        private readonly RedisContext _redisContext;
        public RaceRepository(RedisContext r)
        {
            _redisContext = r;
        }

        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public void AddCandidate()
        {
            throw new NotImplementedException();
        }

        public void addResult()
        {
            throw new NotImplementedException();
        }
    }
}
