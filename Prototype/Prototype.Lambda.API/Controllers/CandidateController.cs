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
    public class CandidateController : ControllerBase
    {
        public const string RouteBase = "api/Candidate";
        private readonly IRedisService redisService;

        public CandidateController(IRedisService r) { redisService = r; }
        // GET: api/Message

        [HttpGet]
        public IEnumerable<CandidateInfo> Get()
        {
            String result = redisService.Get(Const.CandidateKeyBase);
            if (String.IsNullOrWhiteSpace(result)) return null;
            return JsonSerializer.Deserialize<List<CandidateInfo>>(result);
        }

        // GET: api/Candidate/5
        [HttpGet("{id}")]
        public CandidateInfo Get(int id)
        {
            String result = redisService.Get(Const.CandidateKeyBase + id);
            if (String.IsNullOrWhiteSpace(result)) return null;
            return JsonSerializer.Deserialize<CandidateInfo>(result);
        }

    }
}
