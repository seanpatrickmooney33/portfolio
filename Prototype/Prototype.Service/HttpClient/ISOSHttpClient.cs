using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Service.HttpClient
{
    public interface ISOSHttpClient<T> : IDisposable
    {
        IEnumerable<T> GetMultiple(String url);

        IEnumerable<T> GetMultiple<P>(P param, String url);

        T GetSingle(String url);

        T GetSingle<P>(P param, String url);

        void GetVoid(String url);
    }
}
