using Portfolio.Common.Foundation;
using Portfolio.Domain.AggregrateModels.RaceAggregateModel;
using Portfolio.Infastructure.EF.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portfolio.Infastructure.EF.Repositories
{
    public class EFRepository : IRaceRepository
    {
        private readonly EfDbContext _dbContext;
        public IUnitOfWork UnitOfWork => _dbContext;

        public EFRepository(EfDbContext portfolioDbContext)
        {
            _dbContext = portfolioDbContext ?? throw new ArgumentNullException(nameof(portfolioDbContext)); ;
        }


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
