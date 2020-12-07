using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Service.HttpClient.Mock
{
    public static class RouteDefinitionValidator
    {
        public static void Validate(String url, String fieldName)
        {
            if (url.Contains(" ") == true)
            {
                throw new ArgumentException(String.Format("{0} [{1}] cannot contain an empty space.  This resource cannot be found.", fieldName, url));
            }

            if (url.Contains("//") == true)
            {
                throw new ArgumentException(String.Format("{0} [{1}] cannot contain //.", fieldName, url));
            }

            if (url.StartsWith("/") == true)
            {
                throw new InvalidOperationException(String.Format("{0} [{1}] cannot start with /.", fieldName, url));
            }

            if (url.EndsWith("/") == true)
            {
                throw new InvalidOperationException(String.Format("{0} [{1}] cannot end with /.", fieldName, url));
            }
        }
    }
}
