using Prototype.Data;
using EngineServer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Prototype.Service.InMemoryDatabase;
using Prototype.Data.Common;
using EngineServer.Message.File.CMessage;

namespace EngineServer.Message
{
    public class CMessageProcessor : MessageProcessor
    {
        public CMessage CMessage = new CMessage();
        public CMessageProcessor(IRedisBuffer r) :base(r) {}

        protected override void Validate(String FileContent) { }
        protected override void Parse(String FileContent, ElectionTypeEnum overrideFromFileName) 
        {
            List<String> lines = FileContent.Split(System.Environment.NewLine).ToList();

            lines.ForEach(line => {
                if (String.IsNullOrWhiteSpace(line)) return;
                line = line.Remove(0, 1);
                line = line.Remove(line.Length - 1, 1);

                String[] tokens = line.Split("|");
                switch (tokens[0])
                {
                    //TODO
                    // there is an enum for these "row" types
                    case "B":
                        // pasre datetime
                        //tokens[3] = 20200229 dateTime.ToString("yyyyMMdd")
                        //tokens[4] = 1423190  dateTime.ToString("HHmmss") + 0 ?? not sure why the 0 is appened but it is. look back and check it out
                        DateTime.TryParseExact(tokens[3] + tokens[4], "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out CMessage.DateTime);
                        break;
                    case "C":
                        // There is redundant data for Party Type. Based on previous code, tokens[9] is what is used. Not what is in the opcode
                        ContestId contestId = new ContestId(tokens[3]) { ElectionType = overrideFromFileName };

                        /*
                            Props get passed through this area for whatever reason
                            We do not use the Prop Title/ Name in the C Message file any longer; 
                            we use the Prop Title / Name as defined in the R message file exclusively.
                            Prior to new maps, we used the C message Prop Title / Name for maps and the R Message Prop Title / Name everywhere else.
                            With the new maps, we no longer are restricted by the length of the Prop Title/ Name.
                        */
                        if (contestId.RaceType.Equals(RaceTypeEnum.BallotMeasures))
                        {
                            // need to handle this here short name of the prop respective to R message file of the same canidateID. 
                            //R message has long name fo the ballot measure
                            BallotMeasureInfoRecord aaa = new BallotMeasureInfoRecord()
                            {
                                ContestId = contestId,
                                BallotMeasureId = int.Parse(tokens[3].Substring(8, 4)), 
                                FullName = tokens[6],
                                DisplayName = tokens[10],
                            };
                        }
                        else
                        {
                            int CandidateId = (String.IsNullOrWhiteSpace(tokens[4])) ? 0 : int.Parse(tokens[4]);
                            CMessage.candidateInfoRecords.TryAdd(CandidateId, new CandidateInfoRecord()
                            {
                                ContestId = contestId,
                                CandidateId = CandidateId,
                                IsIncumbent = (String.IsNullOrWhiteSpace(tokens[5]) == false),
                                LastName = tokens[6],
                                FirstName = tokens[7],
                                MiddleName = tokens[8],
                                Party = (String.IsNullOrWhiteSpace(tokens[9])) ? PartyTypeEnum.NotAPartyRace : Enum.Parse<PartyTypeEnum>(tokens[9]),
                                DisplayName = tokens[10],
                            });
                        }
                        break;
                }

            });
        }
        protected override void Process()
        {
            // Data normalization happens here
            String value = JsonSerializer.Serialize(CMessage.candidateInfoRecords.Values.ToList().Select(x => (CandidateInfo)x).ToList());
            RedisBuffer.GetBuffer().AddOrUpdate(Const.CandidateKeyBase, value, (key, oldvalue) => { return value; });

            CMessage.candidateInfoRecords.Values.ToList().ForEach(info => {
                value = JsonSerializer.Serialize((CandidateInfo)info);
                RedisBuffer.GetBuffer().AddOrUpdate(Const.CandidateKeyBase + info.CandidateId, value, (key, oldvalue) => { return value; });
            });   
        }
    }
}
