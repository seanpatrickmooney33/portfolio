using Prototype.Lambda.API.Controllers;
using Prototype.Service.HttpClient;
using Prototype.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prototype.Data.Models.VoteResults;
using Prototype.Data.Models.ViewModels;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Prototype.Lambda.API.Service
{
    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _factory = null;
        private readonly ILogger _logger = null;

        public ApiService(IHttpClientFactory factory, ILogger logger)
        {
            this._factory = factory;
            _logger = logger;
        }

        public IEnumerable<CandidateInfo> GetCandidate()
        {
            using (ISOSHttpClient<CandidateInfo> client = _factory.CreateClient<CandidateInfo>())
            {
                return client.GetMultiple(String.Format(CandidateController.RouteBase + "/Get"));
            }
        }

        public CandidateInfo GetCandidate(int id)
        {
            using (ISOSHttpClient<CandidateInfo> client = _factory.CreateClient<CandidateInfo>())
            {
                return client.GetSingle(String.Format(CandidateController.RouteBase + "/Get/{0}", id));
            }
        }

        public IEnumerable<RaceInfoBase> GetRaceInfo()
        {
            using (ISOSHttpClient<RaceInfoBase> client = _factory.CreateClient<RaceInfoBase>())
            {
                return client.GetMultiple(String.Format(RaceController.RouteBase + "/Get"));
            }
        }
        public RaceInfoBase GetRaceInfo(String key)
        {
            using (ISOSHttpClient<RaceInfoBase> client = _factory.CreateClient<RaceInfoBase>())
            {
                return client.GetSingle(String.Format(RaceController.RouteBase + "/Get/{0}", key));

            }
        }
        public RaceInfoBase GetRaceInfo(RedisKey key) 
        {
            return GetRaceInfo(key.ToKey());
        }


        public VoteRecordViewModel GetVoteRecordViewModel(string key)
        {
            using (ISOSHttpClient<VoteRecordViewModel> client = _factory.CreateClient<VoteRecordViewModel>())
            {
                return client.GetSingle(String.Format(ResultController.RouteBase + "/GetVoteRecordViewModel/{0}", key));
            }
        }

        public VoteRecordViewModel GetVoteRecordViewModel(RedisKey key)
        {
            return GetVoteRecordViewModel(key.ToKey());
        }

        public VoteRecord<VoteResults<JudgeResult>> GetJudgeRecord(string key)
        {
            using (ISOSHttpClient<VoteRecord<VoteResults<JudgeResult>>> client = _factory.CreateClient<VoteRecord<VoteResults<JudgeResult>>>())
            {
                return client.GetSingle(String.Format(ResultController.RouteBase + "/GetJudgeRecord/{0}", key));
            }
        }

        public VoteRecord<VoteResults<JudgeResult>> GetJudgeRecord(RedisKey key)
        {
            return GetJudgeRecord(key.ToKey());
        }

        public VoteRecord<RecallResult> GetRecallRecord(string key)
        {
            using (ISOSHttpClient<VoteRecord<RecallResult>> client = _factory.CreateClient<VoteRecord<RecallResult>>())
            {
                return client.GetSingle(String.Format(ResultController.RouteBase + "/GetRecallRecord/{0}", key));
            }
        }

        public VoteRecord<RecallResult> GetRecallRecord(RedisKey key)
        {
            return GetRecallRecord(key.ToKey());
        }

        public VoteRecord<PropResult> GetPropRecord(string key)
        {
            using (ISOSHttpClient<VoteRecord<PropResult>> client = _factory.CreateClient<VoteRecord<PropResult>>())
            {
                return client.GetSingle(String.Format(ResultController.RouteBase + "/GetPropRecord/{0}", key));
            }
        }

        public VoteRecord<PropResult> GetPropRecord(RedisKey key)
        {
            return GetPropRecord(key.ToKey());
        }
    }
}
