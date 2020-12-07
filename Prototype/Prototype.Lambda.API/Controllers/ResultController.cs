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
using Prototype.Data.Models.VoteResults;
using Prototype.Data.Models.ViewModels;

namespace Prototype.Lambda.API.Controllers
{
    [Route(RouteBase)]
    [ApiController]
    public class ResultController : ControllerBase
    {
        public const string RouteBase = "api/Result";
        private readonly IRedisService redisService;

        public ResultController(IRedisService r) { redisService = r; }

        [HttpGet("{Key}")]
        public VoteRecordViewModel GetVoteRecordViewModel(String key)
        {
            String result = redisService.Get(key);
            if (String.IsNullOrWhiteSpace(result)) return null;
            return JsonSerializer.Deserialize<VoteRecordViewModel>(result);
        }

        [HttpGet("{key}")]
        public VoteRecord<VoteResults<JudgeResult>> GetJudgeRecord(String key)
        {
            String result = redisService.Get(key);
            if (String.IsNullOrWhiteSpace(result)) return null;
            return JsonSerializer.Deserialize<VoteRecord<VoteResults<JudgeResult>>>(result);
        }

        [HttpGet("{key}")]
        public VoteRecord<RecallResult> GetRecallRecord(String key)
        {
            String result = redisService.Get(key);
            if (String.IsNullOrWhiteSpace(result)) return null;
            return JsonSerializer.Deserialize<VoteRecord<RecallResult>>(result);
        }

        [HttpGet("{key}")]
        public VoteRecord<PropResult> GetPropRecord(String key)
        {
            String result = redisService.Get(key);
            if (String.IsNullOrWhiteSpace(result)) return null;
            return JsonSerializer.Deserialize<VoteRecord<PropResult>>(result);
        }
    }
}
