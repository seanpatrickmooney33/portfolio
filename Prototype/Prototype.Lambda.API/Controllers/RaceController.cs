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
    public class RaceController : ControllerBase
    {
        public const string RouteBase = "api/Race";
        private readonly IRedisService redisService;

        public RaceController(IRedisService r) { redisService = r; }

        [HttpGet]
        public IEnumerable<RaceInfoBase> Get()
        {
            String result = redisService.Get(Const.RaceKeyBase);
            if (String.IsNullOrWhiteSpace(result)) return null;
            return JsonSerializer.Deserialize<List<RaceInfoBase>>(result);
        }

        [HttpGet("{Key}")]
        public RaceInfoBase Get(String Key)
        {
            RedisKey redisKey = new RedisKey(Key);
            String result = redisService.Get(Const.RaceKeyBase + redisKey.ToKey());
            if (String.IsNullOrWhiteSpace(result)) return null;
            return JsonSerializer.Deserialize<RaceInfoBase>(result);
        }

    }
}
