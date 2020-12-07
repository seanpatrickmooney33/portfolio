using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Service.HttpClient
{
    public interface IServiceSettings
    {
        string ServiceUserName { get; set; }
        string ServicePassword { get; set; }
        string ClientId { get; set; }
        string TokenServiceUrl { get; set; }
        string WebServiceUrl { get; set; }
        int Timeout { get; set; }
    }
}
