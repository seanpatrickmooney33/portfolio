using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Portfolio.Common.Foundation
{
    public interface IUnitOfWork
    {
        Task SaveEntitiesAsync(CancellationToken? cancellationToken);
    }
}
