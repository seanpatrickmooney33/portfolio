using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpecialElection.Service
{
    public enum APIVerb
    {
        upload = 1,
        send = 2
    }
    public class PayLoad
    {

        public String FilePrefix { get; set; }
        public String Message { get; set; }
        public APIVerb Verb { get; set; }
    }

    public interface IMessageClient
    {
        Task SendMessage(PayLoad payLoad);
    }
}
