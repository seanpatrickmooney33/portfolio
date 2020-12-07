using Prototype.Data;
using Prototype.Data.Models.ViewModels;
using Prototype.Data.Models.VoteResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Lambda.API.Service
{
    public interface IApiService
    {
        public IEnumerable<CandidateInfo> GetCandidate();
        public CandidateInfo GetCandidate(int id);

        public IEnumerable<RaceInfoBase> GetRaceInfo();
        public RaceInfoBase GetRaceInfo(String key);
        public RaceInfoBase GetRaceInfo(RedisKey key);

        public VoteRecordViewModel GetVoteRecordViewModel(String key);
        public VoteRecordViewModel GetVoteRecordViewModel(RedisKey key);


        public VoteRecord<VoteResults<JudgeResult>> GetJudgeRecord(String key);
        public VoteRecord<VoteResults<JudgeResult>> GetJudgeRecord(RedisKey key);

        public VoteRecord<RecallResult> GetRecallRecord(String key);
        public VoteRecord<RecallResult> GetRecallRecord(RedisKey key);

        public VoteRecord<PropResult> GetPropRecord(String key);
        public VoteRecord<PropResult> GetPropRecord(RedisKey key);
    }
}
