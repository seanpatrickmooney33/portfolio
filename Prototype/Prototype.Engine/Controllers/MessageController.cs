using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prototype.Data;
using EngineServer.Message;
using EngineServer.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EngineServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly RaceMessageProcessor RMessageProcessor;
        private readonly VMessageProcessor VMessageProcessor;
        private readonly CMessageProcessor CMessageProcessor;
        private readonly Driver Driver;

        public MessageController(RaceMessageProcessor r, VMessageProcessor v, CMessageProcessor c, Driver d) {
            RMessageProcessor = r;
            VMessageProcessor = v;
            CMessageProcessor = c;
            Driver = d;
        }


        // POST: api/Message
        [HttpPost]
        public void UploadVMessage([FromBody] string messageContent, string FileName)
        {
            // need to parse file name to pass through the correct election type
            VMessageProcessor.DoWork(messageContent, FileName);
            Driver.WriteToRedis();
        }

        [HttpPost]
        public void UploadCMessage([FromBody] string messageContent, string FileName)
        {
            // need to parse file name to pass through the correct election type
            CMessageProcessor.DoWork(messageContent, FileName);
        }

        [HttpPost]
        public void UploadRMessage([FromBody] string messageContent, string FileName)
        {
            // need to parse file name to pass through the correct election type
            RMessageProcessor.DoWork(messageContent, FileName);
        }
    }
}
