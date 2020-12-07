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
    public class RaceMessageProcessor : MessageProcessor
    {
        public RaceMessage RMessage = new RaceMessage();
        public RaceMessageProcessor(IRedisBuffer r) : base(r) { }

        protected override void Validate(string FileContent) { }
        protected override void Parse(string FileContent, ElectionTypeEnum overrideFromFileName)
        {
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
                        DateTime.TryParseExact(tokens[3] + tokens[4], "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out RMessage.DateTime); break;
                    case "R":
                        /*                         
                            a.	US Senate - #  U.S. Senate - #
                            b.	United States Representative  U.S. Representative
                            c.	State Assembly Member  State Assembly
                            d.	COA – PJ – District # Division #  Court of Appeal Presiding Justice District # Division #
                            e.	COA – AJ – District # Division # Seat #  Court of Appeal Associate Just District # Division # Seat #

                            Media gets the original format no data normalzation
                         */
                        ContestId contestId = new ContestId(tokens[3]) { ElectionType = overrideFromFileName };


                        if (contestId.RaceType.Equals(RaceTypeEnum.BallotMeasures))
                        {
                            // need to handle this here short name of the prop respective to R message file of the same canidateID. 
                            //R message has long name fo the ballot measure
                            BallotMeasureInfoRecord aaa = new BallotMeasureInfoRecord()
                            {
                                ContestId = contestId,
                                BallotMeasureId = int.Parse(tokens[3].Substring(8, 4)),
                                FullName = tokens[4],
                                DisplayName = tokens[4],
                            };
                        }
                        else 
                        {
                            RMessage.rRecords.Add(new RaceRecord()
                            {
                                ContestId = contestId,
                                RaceName = tokens[4],
                                ElectionShortName = tokens[5],
                            });
                        }
                            
                        break;
                }
            });
        }

        protected override void Process()
        {
            // Data normalization happens here
            
            String value = JsonSerializer.Serialize(
                RMessage.rRecords.Select(x => new RaceInfoBase() { 
                    RaceName = x.RaceName, 
                    RedisKey = new RedisKey(x.ContestId).ToKey()
                }).ToList());

            RedisBuffer.GetBuffer().AddOrUpdate(Const.RaceKeyBase, value, (key, oldvalue) => { return value; });

            RMessage.rRecords.ForEach(info => {
                String myKey = new RedisKey(info.ContestId).ToKey();
                value = JsonSerializer.Serialize(new RaceInfoBase() { 
                    RaceName = info.RaceName, 
                    RedisKey = myKey
                });

                RedisBuffer.GetBuffer().AddOrUpdate(Const.RaceKeyBase + myKey, value, (key, oldvalue) => { return value; });
            });
        }

    }
}
