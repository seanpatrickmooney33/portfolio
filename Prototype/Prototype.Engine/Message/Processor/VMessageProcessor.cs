using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Prototype.Data;
using Prototype.Data.Models.VoteResults;
using EngineServer.Service;
using Prototype.Data.Common;
using Prototype.Data.Models.ViewModels;

namespace EngineServer.Message
{
    public class VMessageProcessor : MessageProcessor
    {
        public VMessage VMessage;
        public VMessageProcessor(IRedisBuffer r) : base(r) { }

        protected override void Validate(String FileContent) { }
        protected override void Parse(String FileContent, ElectionTypeEnum overrideFromFileName)
        {
            VMessage VMessage = new VMessage() {
                ElectionType = overrideFromFileName
            };

            // need to get election type saturated into the opcode
            List<String> lines = FileContent.Split(System.Environment.NewLine).ToList();

            lines.ForEach(line =>
            {
                if (String.IsNullOrWhiteSpace(line)) return;
                line = line.Remove(0, 1);
                line = line.Remove(line.Length - 1, 1);

                String[] tokens = line.Split("|");

                switch (tokens[0])
                {
                    case "B":
                        // pasre datetime
                        //tokens[3] = 20200229 dateTime.ToString("yyyyMMdd")
                        //tokens[4] = 1423190  dateTime.ToString("HHmmss") + 0 ?? not sure why the 0 is appened but it is. look back and check it out
                        DateTime.TryParseExact(tokens[3] + tokens[4], "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out VMessage.DateTime);
                        break;
                    case "A":
                        ARecord aRecord = new ARecord()
                        {
                            County = (CountyTypeEnum)int.Parse(tokens[3]),
                            ReportType = (String.IsNullOrWhiteSpace(tokens[4])) ? ReportingTypeEnum.Unknown : Enum.Parse<ReportingTypeEnum>(tokens[4]),
                            ReportNumber = (String.IsNullOrWhiteSpace(tokens[5]) ? 0 : int.Parse(tokens[5])),
                            CorrectionReportNumber = (String.IsNullOrWhiteSpace(tokens[6]) ? 0 : int.Parse(tokens[6])),
                            PrecinctReporting = (String.IsNullOrWhiteSpace(tokens[7]) ? 0 : int.Parse(tokens[7])),
                            TotalNumberOfPrecinct = (String.IsNullOrWhiteSpace(tokens[8]) ? 0 : int.Parse(tokens[8])),
                            PrecinctReportingPercentage = (String.IsNullOrWhiteSpace(tokens[9]) ? 0.0 : double.Parse(tokens[9])),
                            VoterTurnout = (String.IsNullOrWhiteSpace(tokens[10]) ? 0 : int.Parse(tokens[10])),
                            TotalVoters = (String.IsNullOrWhiteSpace(tokens[11]) ? 0 : int.Parse(tokens[11])),
                            VoterTurnoutPercentage = (String.IsNullOrWhiteSpace(tokens[12]) ? 0.0 : double.Parse(tokens[12])),
                        };
                        if(DateTime.TryParseExact(tokens[13], "yyyyMMddHHmm", null, System.Globalization.DateTimeStyles.None, out DateTime x))
                        {
                            aRecord.FirstReportDateTime = x;
                        }
                        
                        if(DateTime.TryParseExact(tokens[14], "yyyyMMddHHmm", null, System.Globalization.DateTimeStyles.None, out x))
                        {
                            aRecord.LastReportDateTime = x;
                        }
                        
                        
                        VMessage.aRecords.Add(aRecord);
                        break;
                    case "V":
                        ContestId RecordOpCode = new ContestId(tokens[3]) { ElectionType = overrideFromFileName };
                        RecordBase Record = new RecordBase()
                        {
                            ContestId = RecordOpCode,
                            County = (CountyTypeEnum)int.Parse(tokens[4]),
                            PrecinctReporting = int.Parse(tokens[5]),
                            TotalNumberOfPrecinct = int.Parse(tokens[6]),
                            ReportType = (String.IsNullOrWhiteSpace(tokens[7])) ? ReportingTypeEnum.Unknown : Enum.Parse<ReportingTypeEnum>(tokens[7])
                        };

                        if (RecordOpCode.RaceType.Equals(RaceTypeEnum.BallotMeasures))
                        {
                            PropRecord PropRecord = new PropRecord(Record);

                            PropRecord.ResultRecords.Add(new PropResult()
                            {
                                yesVotes = int.Parse(tokens[9]),
                                yesPercent = double.Parse(tokens[10]),
                                noVotes = int.Parse(tokens[12]),
                                noPercent = double.Parse(tokens[13]),
                            });

                            VMessage.PropRecords.Add(PropRecord);
                        }
                        else if (RecordOpCode.RaceType.Equals(RaceTypeEnum.CourtsOfAppeal) 
                                || RecordOpCode.RaceType.Equals(RaceTypeEnum.SupremeCourt))
                        {
                            JudgeRecord JudgeRecord = new JudgeRecord(Record);
                            int index = 8;
                            while (index < tokens.Length)
                            {
                                JudgeRecord.ResultRecords.Add(new JudgeResult()
                                {
                                    CandidateId = int.Parse(tokens[index++]),
                                    yesVotes = int.Parse(tokens[index++]),
                                    yesPercent = double.Parse(tokens[index++]),
                                    noVotes = int.Parse(tokens[index++]),
                                    noPercent = double.Parse(tokens[index++]),
                                });
                            }
                            VMessage.JudgeRecords.Add(JudgeRecord);
                        }
                        else if (RecordOpCode.ElectionType.Equals(ElectionTypeEnum.Recall))
                        {
                            RecallRecord RecallRecord = new RecallRecord(Record);
                            int index = 8;
                            while (index < tokens.Length)
                            {
                                RecallRecord.ResultRecords.Add(new RecallResult()
                                {
                                    yesVotes = int.Parse(tokens[index++]),
                                    yesPercent = double.Parse(tokens[index++]),
                                    noVotes = int.Parse(tokens[index++]),
                                    noPercent = double.Parse(tokens[index++]),
                                });
                            }
                            VMessage.RecallRecords.Add(RecallRecord);
                        }
                        else 
                        {
                            InputCandidateRecord CandidateRecord = new InputCandidateRecord(Record);
                            int index = 8;
                            while(index < tokens.Length)
                            {
                                CandidateRecord.ResultRecords.Add(new CandidateRecord()
                                {
                                    CandidateId = int.Parse(tokens[index++]),
                                    Votes = int.Parse(tokens[index++]),
                                    PartyPercent = double.Parse(tokens[index++]),
                                    RacePercent = double.Parse(tokens[index++]),
                                });
                            }

                            VMessage.CandidateResultRecords.Add(CandidateRecord);
                        }
                        
                        break;
                }
            });
        }

        protected override void Process() 
        {
            // process A records



            // process Statewide and Districtwide Results
            ProcessVoteResults(VMessage.CandidateResultRecords.Where(x => x.County.Equals(CountyTypeEnum.Statewide)).ToList());

            // process County Results
            ProcessVoteResults(VMessage.CandidateResultRecords.Where(x => x.County.Equals(CountyTypeEnum.Statewide) == false).ToList());


            VMessage.JudgeRecords.ForEach(record => {
                
                VoteRecord<VoteResults<JudgeResult>> RaceRecord = new VoteRecord<VoteResults<JudgeResult>>()
                {
                    Key = new RedisKey(record.ContestId, record.County)
                };

                record.ResultRecords.ForEach(result => {
                    RedisBuffer.GetBuffer().TryGetValue(Const.CandidateKeyBase + result.CandidateId, out String value);
                    if (String.IsNullOrWhiteSpace(value))
                    {
                        CandidateInfo candidateInfo = JsonSerializer.Deserialize<CandidateInfo>(value);
                        RaceRecord.VoteResults.Add(new VoteResults<JudgeResult>(candidateInfo, result));
                    }
                });

                String value = JsonSerializer.Serialize(RaceRecord);
                RedisBuffer.GetBuffer().AddOrUpdate(RaceRecord.Key.ToKey(), value, (key, oldValue) => { return value; });
            });

            VMessage.RecallRecords.ForEach(record => {
                VoteRecord<RecallResult> RaceRecord = new VoteRecord<RecallResult>()
                {
                    Key = new RedisKey(record.ContestId, record.County),
                    VoteResults = record.ResultRecords,
                };

                
                String value = JsonSerializer.Serialize(RaceRecord);
                RedisBuffer.GetBuffer().AddOrUpdate(RaceRecord.Key.ToKey(), value, (key, oldValue) => { return value; });
            });

            VMessage.PropRecords.ForEach(record => {
                VoteRecord<PropResult> RaceRecord = new VoteRecord<PropResult>()
                {
                    Key = new RedisKey(record.ContestId, record.County),
                    VoteResults = record.ResultRecords,
                };

                String value = JsonSerializer.Serialize(RaceRecord);
                RedisBuffer.GetBuffer().AddOrUpdate(RaceRecord.Key.ToKey(), value, (key, oldValue) => { return value; });
            });

            //ProcessRaceEndpoints();
        }

        private void ProcessVoteResults(List<InputCandidateRecord> CandidateResultRecords)
        {
            CandidateResultRecords.ForEach(record => {

                VoteRecordViewModel RaceRecord = new VoteRecordViewModel()
                {
                    Key = new RedisKey(record.ContestId, record.County)
                };

                #region get Race Name

                RedisKey RaceKey = new RedisKey(RaceRecord.Key)
                {
                    CountyType = CountyTypeEnum.NotACounty,
                };
                RaceInfoBase race = new RaceInfoBase();
                if (RedisBuffer.GetBuffer().TryGetValue(Const.RaceKeyBase + RaceKey.ToKey(), out String raceResult))
                {
                    race = JsonSerializer.Deserialize<RaceInfoBase>(raceResult);
                    RaceRecord.RaceName = race.RaceName;
                }

                #endregion get Race Name

                #region Get GreaterAreaRecord

                VoteRecordViewModel GreaterAreaRecord = null;

                if (RaceRecord.Key.CountyType.Equals(CountyTypeEnum.Statewide) == false)
                {
                    if (RedisBuffer.GetBuffer().TryGetValue(new RedisKey(RaceRecord.Key) { CountyType = CountyTypeEnum.Statewide }.ToKey(), 
                        out String GreateAreaResult))
                    {
                        GreaterAreaRecord = JsonSerializer.Deserialize<VoteRecordViewModel>(GreateAreaResult);
                    }
                }

                #endregion Get GreaterAreaRecord

                #region Populate Each entry
                
                record.ResultRecords.ForEach(result => {

                    VoteResultViewModel voteResultViewModel = new VoteResultViewModel();

                    if (RedisBuffer.GetBuffer().TryGetValue(Const.CandidateKeyBase + result.CandidateId, out String CandidateInfoResult))
                    {
                        CandidateInfo candidateInfo = JsonSerializer.Deserialize<CandidateInfo>(CandidateInfoResult);
                        voteResultViewModel.CandidateDisplayName = candidateInfo.DisplayName;
                        voteResultViewModel.CandidateParty = candidateInfo.Party;

                        voteResultViewModel.LocalVote = result.Votes;
                        voteResultViewModel.LocalPartyPercent = result.PartyPercent;
                        voteResultViewModel.LocalRacePercent = result.RacePercent;

                        if(GreaterAreaRecord != null)
                        {
                            VoteResultViewModel greaterAreaViewModel = GreaterAreaRecord.VoteResults.First(x => x.CandidateDisplayName.Equals(voteResultViewModel.CandidateDisplayName));
                            voteResultViewModel.GreaterAreaVote = greaterAreaViewModel.LocalVote;
                            voteResultViewModel.GreaterAreaPartyPercent = greaterAreaViewModel.LocalPartyPercent;
                            voteResultViewModel.GreaterAreaRacePercent = greaterAreaViewModel.LocalRacePercent;
                        }

                        RaceRecord.VoteResults.Add(voteResultViewModel);
                    }
                });
                
                #endregion Populate Each entry

                String value = JsonSerializer.Serialize<VoteRecordViewModel>(RaceRecord);



                RedisBuffer.GetBuffer().AddOrUpdate(RaceRecord.Key.ToKey(), value, (key, oldValue) => { return value; });

                String uri = "/returns";

                if (EnumHelper<RaceTypeEnum>.GetName(RaceRecord.Key.RaceType).Equals(RaceTypeEnum.President))
                {
                    uri = "/president/party/" + EnumHelper<PartyTypeEnum>.GetName(RaceRecord.Key.PartyType).ToLower().Replace(" ", "-");

                } else
                {
                    uri += "/" + EnumHelper<RaceTypeEnum>.GetName(RaceRecord.Key.RaceType).ToLower().Replace(" ", "-");
                }

                if (RaceRecord.Key.DistrictId == 0 && RaceRecord.Key.CountyType.Equals(CountyTypeEnum.Statewide) == false) 
                { 
                    uri += "/county/" + EnumHelper<CountyTypeEnum>.GetAbbreviation(RaceRecord.Key.CountyType);
                }
                else if (RaceRecord.Key.DistrictId != 0 && RaceRecord.Key.CountyType.Equals(CountyTypeEnum.Statewide))
                {
                    uri += "/district/" + RaceRecord.Key.DistrictId;
                } else if (RaceRecord.Key.DistrictId != 0 && RaceRecord.Key.CountyType.Equals(CountyTypeEnum.Statewide) == false)
                {
                    uri += "/district/" + RaceRecord.Key.DistrictId + "/county/" + EnumHelper<CountyTypeEnum>.GetAbbreviation(RaceRecord.Key.CountyType);
                }

                RedisBuffer.GetBuffer().AddOrUpdate(uri, value, (key, oldValue) => { return value; });

            });
        }
    
        //private void ProcessRaceEndpoints()
        //{
        //    List<RaceInfoBase> races = new List<RaceInfoBase>();
        //    if (RedisBuffer.GetBuffer().TryGetValue(Const.RaceKeyBase, out String result))
        //    {
        //        races = JsonSerializer.Deserialize<List<RaceInfoBase>>(result);
        //    }

        //    races.ForEach(race => {
        //        RedisKey redisKey = new RedisKey(race.RedisKey);

        //        // statewide
        //        redisKey.CountyType = CountyTypeEnum.Statewide;
        //        VoteRecordViewModel test = new VoteRecordViewModel();
        //        if (RedisBuffer.GetBuffer().TryGetValue(redisKey.ToKey(), out String result))
        //        {
        //            //test = JsonSerializer.Deserialize<VoteRecordViewModel>(result);
        //            /*
        //             need to add these to the results. Though adding this data during ProcessVoteResults might be better
        //                    "raceTitle": "President Democratic - Butte County Results",
        //                    "Reporting": "100% (165 of 165) precincts reporting",
        //                    "ReportingTime": "March 26, 2020, 11:09 a.m.",
        //             */

        //            RedisBuffer.GetBuffer().AddOrUpdate("returns/president/party/democratic", result, (key, oldValue) => { return result; });
        //            // write
        //        }
        //        // county statewide

        //        // distict statewide
        //        // district county
        //    });


        //}
    }
}
