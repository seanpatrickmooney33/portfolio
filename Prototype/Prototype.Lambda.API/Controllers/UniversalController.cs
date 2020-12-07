using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prototype.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prototype.Service.InMemoryDatabase;
using Prototype.Data;
using System.Text.Json;
using Prototype.Data.Common;

namespace Prototype.Lambda.API.Controllers
{
    [Route(RouteBase)]
    [ApiController]
    public class UniversalController : ControllerBase
    {
        public const string RouteBase = "";
        private readonly IRedisService redisService;

        public UniversalController(IRedisService r) { redisService = r; }


        [HttpGet]
        public ContentResult Default()
        {
            String result = redisService.Get(Request.Path);
            return new ContentResult()
            {
                Content = result,
                StatusCode = 200,
                ContentType = "application/json; charset=UTF-8"
            };
        }
    }
}
