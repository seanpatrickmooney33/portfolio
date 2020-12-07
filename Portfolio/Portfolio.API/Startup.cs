using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Portfolio.API
{
    [ExcludeFromCodeCoverage]
    public class Startup : FunctionsStartup
    {  
        public override void Configure(IFunctionsHostBuilder builder)
        {
        }
    }
}
