using SpecialElection.Service;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Prototype.Test.Utility
{
    public class MockPayload
    {
        public String FileName, FileParamName, FileContent, FileKeyName, Endpoint;
    }

    public class MockMessageClient : IMessageClient
    {

        private readonly static HttpClient client = new HttpClient();

        public static async Task SendMessage(MockPayload payload)
        {
            using MultipartFormDataContent content = new MultipartFormDataContent
            {
                { new StringContent(payload.FileContent, System.Text.Encoding.UTF8), payload.FileParamName, payload.FileName },
                { new StringContent("a34fjfweflml3r3qdf43f9v9f434f43", System.Text.Encoding.UTF8), payload.FileKeyName }
            };

            // post to server
            HttpResponseMessage httpResponseMessage = await client.PostAsync("http://172.27.167.91/" + payload.Endpoint, content);
            httpResponseMessage.EnsureSuccessStatusCode();
        }

        public async Task SendMessage(PayLoad payLoad)
        {
            DateTime dt = DateTime.UtcNow;

            using MultipartFormDataContent cContent = new MultipartFormDataContent
            {
                { new StringContent(TestData.Cmsg, System.Text.Encoding.UTF8), "file", "C20PP.txt" },
                { new StringContent("a34fjfweflml3r3qdf43f9v9f434f43", System.Text.Encoding.UTF8), "key" }
            };

            // post to server
            HttpResponseMessage httpResponseMessage = await client.PostAsync("http://172.27.167.91//send", cContent);
            httpResponseMessage.EnsureSuccessStatusCode();


            using MultipartFormDataContent rContent = new MultipartFormDataContent
            {
                { new StringContent(TestData.Rmsg, System.Text.Encoding.UTF8), "file", "R20SE.txt" },
                { new StringContent("a34fjfweflml3r3qdf43f9v9f434f43", System.Text.Encoding.UTF8), "key" }
            };

            // post to server
            httpResponseMessage = await client.PostAsync("http://172.27.167.91//send", rContent);
            httpResponseMessage.EnsureSuccessStatusCode();


            using MultipartFormDataContent vContent = new MultipartFormDataContent
            {
                { new StringContent(TestData.Vmsg, System.Text.Encoding.UTF8), "messageFile", "V20SE.txt" },
               { new StringContent("a34fjfweflml3r3qdf43f9v9f434f43", System.Text.Encoding.UTF8), "token" }
            };

            // post to server
            httpResponseMessage = await client.PostAsync("http://172.27.167.91//upload", vContent);
            httpResponseMessage.EnsureSuccessStatusCode();

        }


    }
}
