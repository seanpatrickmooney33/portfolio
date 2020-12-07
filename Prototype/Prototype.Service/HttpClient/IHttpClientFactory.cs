using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Service.HttpClient
{
    public interface IHttpClientFactory
    {
        ISOSHttpClient<T> CreateClient<T>();
    }
}
