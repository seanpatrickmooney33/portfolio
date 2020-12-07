using System;
using System.Collections.Generic;
using System.Text;

namespace Portfolio.Common.Foundation
{

    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
