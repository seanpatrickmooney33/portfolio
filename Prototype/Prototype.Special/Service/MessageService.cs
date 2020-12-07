using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using SpecialElection.Data;
using Prototype.Data;
using SpecialElection.Data.Model;

namespace SpecialElection.Service
{
    public class MessageService
    {
        private readonly ApplicationDbService ApplicationDbService;
        private IMessageClient MessageClient { get; }
        public MessageService(ApplicationDbService a, IMessageClient messageClient) 
        { 
            this.ApplicationDbService = a;
            this.MessageClient = messageClient;
        }

        public async Task<String> GenerateRMSG()
        {
            List<Race> activeRaceList = await this.ApplicationDbService.GetRaces().Include(r => r.Election)
                                                                    .Where(r => r.Election.IsActive == true)
                                                                    .OrderBy(x => x.Type)
                                                                    .ToListAsync();

            int index = 1;

            DateTime dateTime = DateTime.UtcNow;
            String rmsg = "[B|CAS|" + (index++) + "|" + dateTime.ToString("yyyyMMdd") + "|" + dateTime.ToString("HHmmss") + "0]\n";

            foreach (Race race in activeRaceList)
            { 
                ContestId opCode = new ContestId() { 
                    RaceType = race.Type,
                    ElectionType = ElectionTypeEnum.Special,
                    DistrictId = race.District
                };

                //[R|CAS|2|110100250000|U.S. House of Representatives District 25|SE]
                rmsg += "[R|CAS|" + (index++) + "|" + opCode.ToOpCode() + "|" + race.Name + "|SE]\n";
            }


            rmsg += "[E|CAS|" + index + "|" + dateTime.ToString("yyyyMMdd") + "|" + dateTime.ToString("HHmmss") + "0]\n";
            return rmsg;
        }
    
        public async Task<String> GenerateCMSG()
        {
            int index = 1;

            List<Race> races = await this.ApplicationDbService.GetRaces()
                .Include(x => x.Election)
                .Where(x => x.Election.IsActive == true)
                .Include(x => x.Candidates)
                .OrderBy(x => x.Id)
                .ToListAsync();

            DateTime dateTime = DateTime.UtcNow;
            String cmsg = "[B|CAS|" + (index++) + "|" + dateTime.ToString("yyyyMMdd") + "|" + dateTime.ToString("HHmmss") + "0]\n";

            foreach(Race race in races) { 
                foreach (Candidate candidate in race.Candidates)
                {
                    ContestId opCode = new ContestId()
                    {
                        RaceType = candidate.Race.Type,
                        ElectionType = ElectionTypeEnum.Special,
                        DistrictId = candidate.Race.District
                    };

                    cmsg += "[C|CAS|" + (index++);
			        cmsg += "|" + opCode.ToOpCode();
			        cmsg += "|" + candidate.Id;
			        cmsg += "|" + "";  //TODO need to add incumbent to db
			        cmsg += "|" + candidate.LastName; 
			        cmsg += "|" + candidate.FirstName;  
			        cmsg += "|" + candidate.MiddleName;
                    String candidateParty = ((int)candidate.Party).ToString();
                    cmsg += "|" + ((candidateParty.Length == 1) ? "0" + candidateParty : candidateParty);
			        cmsg += "|" + candidate.DisplayName;
			        cmsg += "]\n";
                }
            }

            cmsg  += "[E|CAS|"+ index + "|" + dateTime.ToString("yyyyMMdd") + "|" + dateTime.ToString("HHmmss") + "0]\n";
            return cmsg;
        }
    
        public async Task<String> GeneratePMSG()
        {
            int index = 1;
            DateTime dateTime = DateTime.UtcNow;

            String pmsg = "(B|CAS|" + (index++) + "|" + dateTime.ToString("yyyyMMdd") + "|" + dateTime.ToString("HHmmss") + "0)\n";

            //Record -> (P|CAS|2|010001000000|59|58000)
            List<Race> RaceList = await this.ApplicationDbService.GetRaces().Include(x => x.RaceCountyDataList).ToListAsync();

            foreach(Race race in RaceList)
            {
                int totalPrecinctForRace = race.RaceCountyDataList.Sum(x => x.NumberOfPrecinct);
                //get candidate data

			    pmsg += "(P|CAS|" + (index++);
			    pmsg += "|" + race.Id; // this might supposed to be race type
			    pmsg += "|59";
			    pmsg += "|" + totalPrecinctForRace;
                pmsg += ")\n";

                foreach (RaceCountyData raceCountyData in race.RaceCountyDataList)
                {
					pmsg += "(P|CAS|" + (index++);
					pmsg += "|" + race.Id; // this might supposed to be race type
                    pmsg += "|" + (int)raceCountyData.County;
					pmsg += "|" + raceCountyData.NumberOfPrecinct;
                    pmsg += ")\n";
                }
            }

            pmsg += "(E|CAS|" + index + "|" + dateTime.ToString("yyyyMMdd") + "|" + dateTime.ToString("HHmmss") + "0)\n";
            return pmsg;
        }
    
        public String GenerateSMSG()
        {
            int index = 1;
            DateTime dateTime = DateTime.UtcNow;
            String smsg = "[B|CAS|" + (index++) + "|" + dateTime.ToString("yyyyMMdd") + "|" + dateTime.ToString("HHmmss") + "0]\n";

            // get all CountyTypeEnums as a list value
            List<CountyTypeEnum> countyInfoList = new List<CountyTypeEnum>();
            foreach (CountyTypeEnum countyInfo in countyInfoList) {
                string id = ((int)countyInfo).ToString();
			    smsg += "[S|CAS|" + (index++) + "|" + ((id.Length == 0) ? "0" + id : id) + "|"+ countyInfo + "]\n";
            }

            smsg += "[E|CAS|" + (index++) + "|" + dateTime.ToString("yyyyMMdd") + "|" + dateTime.ToString("HHmmss")+"0]\n";
            return smsg;
        }
    
        public async Task<String> GenerateVMSG()
        {
            DateTime dateTime = DateTime.UtcNow;
            #region Organize Data

            Election election = await this.ApplicationDbService.GetElection().Where(x => x.IsActive)
                                                                         .Include(x => x.Races).ThenInclude(r => r.Candidates)
                                                                         .Include(x => x.Races).ThenInclude(c => c.RaceCountyDataList).ThenInclude(x => x.CandidateResults)
                                                                         .FirstOrDefaultAsync();

            Dictionary<int, RaceCountyData> aggregateCountyDataDictionary = new Dictionary<int, RaceCountyData>();

            election.Races = election.Races.OrderBy(r => r.Id).ToList();
            foreach (Race race in election.Races)
            {
                List<RaceCountyData> raceCountyDataList = this.ApplicationDbService.GetRaceCountyData().Where(r => r.RaceId.Equals(race.Id))
                                                                                                .Include(x => x.CandidateResults).ThenInclude(x => x.Candidate)
                                                                                                .OrderBy(r => r.County)
                                                                                                .ToList();

                race.RaceCountyDataList =  race.RaceCountyDataList.OrderBy(r => r.County).ToList();
                foreach (RaceCountyData raceCountyData in raceCountyDataList)
                {
                    raceCountyData.CandidateResults = raceCountyData.CandidateResults
                                                                     .OrderBy(r => r.Candidate.DisplayOrder)
                                                                     .ToList();

                    if (aggregateCountyDataDictionary.TryGetValue(raceCountyData.Id, out RaceCountyData aggregateData))
                    {
                        aggregateData.NumberOfPrecinct += raceCountyData.NumberOfPrecinct;
                        aggregateData.PrecinctsReporting += raceCountyData.PrecinctsReporting;
                    }
                    else { aggregateCountyDataDictionary.Add(raceCountyData.Id, raceCountyData); }
                }
            }


            RaceCountyData statewideCountyData = new RaceCountyData() { County = CountyTypeEnum.Statewide };
            List<RaceCountyData> aggregateCountyDataList = aggregateCountyDataDictionary.Values.ToList();

            statewideCountyData.PrecinctsReporting = aggregateCountyDataList.Sum(x => x.PrecinctsReporting);
            statewideCountyData.NumberOfPrecinct = aggregateCountyDataList.Sum(x => x.NumberOfPrecinct);
            statewideCountyData.UpdatedDateTime = aggregateCountyDataList.Max(x => x.UpdatedDateTime);
            aggregateCountyDataList.Add(statewideCountyData);

            aggregateCountyDataList = aggregateCountyDataList.OrderBy(x => (int)x.County).ToList();

            #endregion Organize Data

            #region Create A Message

            int index = 1;
            // SOF -> (B|CAS|1|20090421|1328200)

            String vmsg = "[B|CAS|" + (index++) + "|" + dateTime.ToString("yyyyMMdd") + "|" + dateTime.ToString("HHmmss") + "0]\n";

            foreach (RaceCountyData aggregateCountyData in aggregateCountyDataList) {
                // [A|CAS|2|19||||180|180|||||202003171641|202003171641]
                vmsg += "[A|CAS";
			    vmsg += "|" + (index++);
			    vmsg += "|" + ((((int)aggregateCountyData.County).ToString().Length == 1) ? "0" + ((int)aggregateCountyData.County).ToString() : ((int)aggregateCountyData.County).ToString()); // county id
			    vmsg += "|";  //report_type
			    vmsg += "|";  // report number
			    vmsg += "|";  // correction report number
			    vmsg += "|" + aggregateCountyData.PrecinctsReporting;               //aggregate precincts_reporting for county from results
                vmsg += "|" + aggregateCountyData.NumberOfPrecinct;
			    vmsg += "|"; // precincts reporting percentage
			    vmsg += "|"; //$something;//$votes_turnout;
			    vmsg += "|"; //$votes_total;   //can't know this until process all the candidates (sigh)
			    vmsg += "|"; //$votes_turnout percentage;
                vmsg += "|" + aggregateCountyData.UpdatedDateTime.ToString("yyyyMMddHHmm");//date_format(aggregateCountyData.DateAdded, "YmdHi"); //201210030951           
                vmsg += "|" + aggregateCountyData.UpdatedDateTime.ToString("yyyyMMddHHmm");//date_format(aggregateCountyData.DateAdded, "YmdHi");
                                 //$content .= '|';   /* report type */
                vmsg += "]\n";
            };
		
		    // EOF ->(E|CAS|8|20090421|1332030)
            vmsg += "[E|CAS|" + (index++) + "|" + dateTime.ToString("yyyyMMdd") + "|" + dateTime.ToString("HHmmss") + "0]\n";

            #endregion Create A Records

            #region Create V Records
            index = 1;
            // [B|CAS|1|20200317|1641320]
            vmsg += "[B|CAS|"+ (index++) + "|" + dateTime.ToString("yyyyMMdd") + "|" + dateTime.ToString("HHmmss") + "0]\n";

            //do 59 entries for each race.
            foreach (Race raceData in election.Races) 
		    {
			    int myTotal = 0;
			
                ContestId opCode = new ContestId()
                {
                    RaceType = raceData.Type,
                    ElectionType = ElectionTypeEnum.Special,
                    DistrictId = raceData.District
                };

                Dictionary<int, CandidateResult> aggregateCandidateDataDictionary = new Dictionary<int, CandidateResult>();
			    int myReportingPrecincts = 0;
			    int myTotalPrecincts = 0;

                foreach (RaceCountyData countyData in raceData.RaceCountyDataList )
			    {
                    foreach (CandidateResult candidateData in countyData.CandidateResults)
                    {
                        myTotal += candidateData.Votes;

                        if (aggregateCandidateDataDictionary.TryGetValue(candidateData.CandidateId, out CandidateResult aggregateCandidateData))
                        {
                            aggregateCandidateData.Votes += candidateData.Votes;
                        }
                        else { aggregateCandidateDataDictionary.Add(candidateData.CandidateId, candidateData.Clone()); }
                    }
				    myReportingPrecincts += countyData.PrecinctsReporting;
				    myTotalPrecincts += countyData.NumberOfPrecinct;
                }

                // [V|CAS|2|120100280000|59|402|402||214|5912|2.9|2.9|215|47516|23.5|23.5|216|42222|20.9|20.9|217|81918|40.5|40.5|218|24536|12.1|12.1]
                vmsg += "[V|CAS";
			    vmsg += "|" + (index++);
			    vmsg += "|" + opCode.ToOpCode();
			    vmsg += "|59";
			    vmsg += "|" + myReportingPrecincts;
			    vmsg += "|" + myTotalPrecincts;
			    vmsg += "|"; //.$report_type;

                foreach(CandidateResult candidateData in aggregateCandidateDataDictionary.Values)
                {
                    vmsg += "|" + candidateData.CandidateId;
                    vmsg += "|" + candidateData.Votes;
                    double percent = 0.0;
                    if (candidateData.Votes != 0){
                        percent = (((double)candidateData.Votes) / ((double)myTotal) * 100);
                    }

				    vmsg += "|" + String.Format("{0:F1}", percent);
                    // yes this is duplicate. and yes this is correct for reasons that are lost to time.
                    vmsg += "|" + String.Format("{0:F1}", percent);
                }
			    
                vmsg += "]\n";

                    //do entries for each county.
                foreach (RaceCountyData countyData in raceData.RaceCountyDataList)
			    {
				    vmsg += "[V|CAS";
				    vmsg += "|" + (index++);
				    vmsg += "|" + opCode.ToOpCode();
				    vmsg += "|" + (int)countyData.County;
				    vmsg += "|" + countyData.PrecinctsReporting;
				    vmsg += "|" + countyData.NumberOfPrecinct;				
				    vmsg += "|"; //.$report_type;

                    foreach (CandidateResult candidateData in countyData.CandidateResults)
                    {
                        vmsg += "|" + candidateData.CandidateId;
                        vmsg += "|" + candidateData.Votes;

                        // weird observed behavior. probably bugs that have been existed forever
						vmsg += "|" + String.Format("{0:F1}", candidateData.Percent) + "0"; 
                        vmsg += "|" + String.Format("{0:F1}", candidateData.Percent) + "0";
                    }

				    vmsg += "]\n";
                }
            }
           
            // EOF ->(E|CAS|8|20090421|1332030)
            vmsg += "[E|CAS|" + index + "|" + dateTime.ToString("yyyyMMdd") + "|" + dateTime.ToString("HHmmss") + "0]\n";

            #endregion Create V Records

            return vmsg;
        }

    }
}
