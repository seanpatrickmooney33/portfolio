using Microsoft.EntityFrameworkCore;
using Portfolio.Common.Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Portfolio.Infastructure.EF.Context
{
    public class EfDbContext : DbContext, IUnitOfWork
    {
        public async Task SaveEntitiesAsync(CancellationToken? cancellationToken)
        {
            await this.SaveChangesAsync(cancellationToken.Value);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            // build database relations for data set. Primary keys, foreign keys, indexes, constratins. 
            base.OnModelCreating(modelBuilder);
        }
    }
}
