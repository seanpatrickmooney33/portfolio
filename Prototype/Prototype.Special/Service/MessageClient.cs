using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpecialElection.Service
{
    public class MessageClient : IMessageClient
    {
        private IConfiguration Configuration { get; }
        public MessageClient(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task SendMessage(PayLoad payLoad)
        {
            try
            {
                String protocal = Configuration.GetValue<String>("ServerProtocal");
                List<String> EngineServerDNS = Configuration.GetValue<List<String>>("EngineServerDNS");
                DateTime dt = DateTime.UtcNow;
                String fileName = payLoad.FilePrefix + dt.ToString("yy") + "SE" + ".txt";

                using HttpClient client = new HttpClient();
                using MultipartFormDataContent content = new MultipartFormDataContent
                {
                    { new StringContent(payLoad.Message, System.Text.Encoding.UTF8), "file", fileName }
                };

                EngineServerDNS.ForEach(async dns =>
                {
                    // post to server
                    String uri = protocal + @"://" + dns + "/" + payLoad.Verb.ToString();

                    HttpResponseMessage httpResponseMessage = await client.PostAsync(uri, content);
                    httpResponseMessage.EnsureSuccessStatusCode();
                });
            }
            catch
            {
                // logging and error handling
            }
        }

    }
}
