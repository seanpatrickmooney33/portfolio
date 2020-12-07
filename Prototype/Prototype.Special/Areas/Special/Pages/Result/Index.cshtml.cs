using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SpecialElection.Data;
using Prototype.Data;
using SpecialElection.Data.Model;

namespace SpecialElection.Areas.Special.Pages.ResultView
{
    public class IndexModel : BasePageModel
    {
        public IndexModel(ApplicationDbService context) : base(context) { }

        public Election Election { get; set; }
        public Dictionary<int, DistrictwideModel> DistrictwideData { get; set; }
        public int TotalPrecinctsReporting { get; set; }
        public int TotalNumberOfPrecinct { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            Election = await _dbService.GetElection().Where(x => x.IsActive)
                                        .Include(x => x.Races).ThenInclude(x => x.RaceCountyDataList).ThenInclude(x => x.CandidateResults)
                                        .Include(x => x.Races).ThenInclude(x => x.Candidates)
                                        .FirstOrDefaultAsync();

            if (Election == null)
            {
                return NotFound();
            }


            DistrictwideData = new Dictionary<int, DistrictwideModel>();

            foreach (Race race in Election.Races)
            {
                DistrictwideModel model = new DistrictwideModel();

                foreach (Candidate candidate in race.Candidates)
                {
                    model.Data.Add(candidate.Id, new DistrictwideCandidateData() { 
                        DisplayName = candidate.DisplayName,
                        Party = candidate.Party
                    });
                }

                if (race.RaceCountyDataList != null && race.RaceCountyDataList.Count() > 0) {
                    model.TotalNumberOfPrecinct = race.RaceCountyDataList.Sum(x => x.NumberOfPrecinct);
                    model.TotalPrecinctsReporting = race.RaceCountyDataList.Sum(x => x.PrecinctsReporting);

                    foreach (RaceCountyData result in race.RaceCountyDataList)
                    {
                        foreach (CandidateResult candidateResult in result.CandidateResults)
                        {
                            model.TotalVotes += candidateResult.Votes;
                            if (model.Data.TryGetValue(candidateResult.CandidateId, out DistrictwideCandidateData candidate))
                            {
                                candidate.Votes += candidateResult.Votes;
                            }
                            else 
                            {
                                model.Data.Add(candidateResult.CandidateId, new DistrictwideCandidateData()
                                {
                                    Votes = candidateResult.Votes,
                                    DisplayName = candidateResult.Candidate.DisplayName,
                                    Party = candidateResult.Candidate.Party,
                                    DisplayOrder = candidateResult.Candidate.DisplayOrder
                                }); 
                            }
                        }

                        if (model.TotalVotes != 0)
                        {
                            foreach (DistrictwideCandidateData districtwideCandidateData in model.Data.Values)
                            {
                                districtwideCandidateData.VotePercent = String.Format("{0:F1}", (double)districtwideCandidateData.Votes / model.TotalVotes * 100);
                            }
                        }
                    }
                }

                DistrictwideData.Add(race.Id, model);
            } 

            return Page();
        }

    }


    public class DistrictwideModel
    {
        public int TotalPrecinctsReporting { get; set; } = 0;
        public int TotalNumberOfPrecinct { get; set; } = 0;
        public int TotalVotes { get; set; } = 0;
        public Dictionary<int, DistrictwideCandidateData> Data { get; set; } = new Dictionary<int, DistrictwideCandidateData>();
    }

    public class DistrictwideCandidateData
    {
        public String DisplayName { get; set; }
        public PartyTypeEnum Party { get; set; }
        public int DisplayOrder { get; set; }
        public int Votes { get; set; } = 0;
        public String VotePercent { get; set; } = "0.0";
    }
}

