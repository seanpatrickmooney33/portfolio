using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpecialElection.Data;
using Prototype.Data;
using SpecialElection.Data.Model;

namespace SpecialElection.Areas.Special.Pages.ResultView
{
    public class CreateModel : BasePageModel
    {
        public CreateModel(ApplicationDbService context) : base(context) { }

        [BindProperty]
        public InputModel inputModel { get; set; }

        public class InputModel
        {
            public int raceCountyDataId { get; set; }
            public CountyTypeEnum County { get; set; }
            public String RaceName { get; set; }
            public int PrecinctsReporting { get; set; }
            [Required]
            [Range(0, int.MaxValue)]
            public int NumberOfPrecinct { get; set; }
            public List<ResultModel> ResultModelList { get; set; } = new List<ResultModel>();
        }

        public class ResultModel
        {
            [Required]
            public int CandidateId { get; set; }
            public String CandidateDisplayName { get; set; }
            public PartyTypeEnum Party { get; set; }
            [Required]
            [Range(0, int.MaxValue)]
            public int Votes { get; set; }
            [Required]
            [Range(0, 100.0)]
            public double Percent { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int? raceCountyDataId)
        {
            if(raceCountyDataId.HasValue == false)
            {
                return NotFound();
            }

            RaceCountyData raceCountyData = await _dbService.GetRaceCountyData()
                                                                    .Include(x => x.Race)
                                                                    .ThenInclude(r => r.Candidates)
                                                                    .FirstOrDefaultAsync(x => x.Id.Equals(raceCountyDataId.Value));

            if(raceCountyData == null)
            {
                return NotFound();
            }

            inputModel = new InputModel()
            {
                raceCountyDataId = raceCountyData.Id,
                RaceName = raceCountyData.Race.Name,
                NumberOfPrecinct = raceCountyData.NumberOfPrecinct,
                PrecinctsReporting = raceCountyData.PrecinctsReporting,
                County = raceCountyData.County
            };

            foreach(Candidate candidate in raceCountyData.Race.Candidates)
            {
                inputModel.ResultModelList.Add(new ResultModel() { 
                    CandidateId = candidate.Id,
                    CandidateDisplayName = candidate.DisplayName,
                    Party = candidate.Party
                });
            }

            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                RaceCountyData raceCountyData = await _dbService.GetRaceCountyData()
                                                                        .Include(x => x.CandidateResults)
                                                                        .FirstAsync(x => x.Id.Equals(inputModel.raceCountyDataId));

                if (raceCountyData == null) { return Page(); }

                raceCountyData.PrecinctsReporting = inputModel.PrecinctsReporting;
                await _dbService.EditRaceCountyData(raceCountyData);

                inputModel.ResultModelList.ForEach(x => {
                    CandidateResult temp = raceCountyData.CandidateResults.FirstOrDefault(z => z.CandidateId.Equals(x.CandidateId));
                    if(temp != null)
                    {
                        temp.Votes = x.Votes;
                        temp.Percent = x.Percent;
                    }
                });

                await _dbService.EditCandidateResults(raceCountyData.CandidateResults);


                return RedirectToPage("../CurrentElection/Index");

            }
            catch { }

            return Page();
        }
    }
}
