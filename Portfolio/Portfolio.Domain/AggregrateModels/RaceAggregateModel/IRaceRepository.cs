using Portfolio.Common.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portfolio.Domain.AggregrateModels.RaceAggregateModel
{
    public interface IRaceRepository : IRepository<Race>
    {
        void AddCandidate();
        void addResult();
    }
}
