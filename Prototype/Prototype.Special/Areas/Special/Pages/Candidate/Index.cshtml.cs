using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SpecialElection.Data;
using SpecialElection.Data.Model;

namespace SpecialElection.Areas.Special.Pages.CandidateView
{
    public class IndexModel : BasePageModel
    {
        [BindProperty]
        public List<InputModel> Candidate { get; set; }

        [BindProperty]
        public int RaceId { get; set; }

        public String RaceName { get; set; }


        public IndexModel(ApplicationDbService context) : base(context) { }

        public class InputModel : Candidate
        {
            public InputModel() { }
            public InputModel(Candidate c) : base(c){ }
        }

        public async Task<IActionResult> OnGetAsync(int? RaceId)
        {
            if (RaceId == null)
            {
                return NotFound();
            }
            Race race = await _dbService.GetRaces().Include(x => x.Candidates).FirstOrDefaultAsync(x => x.Id.Equals(RaceId.Value));

            if(race == null)
            {
                return NotFound();
            }

            this.RaceId = race.Id;
            this.RaceName = race.Name;

            Candidate = race.Candidates.Select(x => new InputModel(x.Clone())).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteCandidateAsync(int? CandidateId)
        {
            if (CandidateId.HasValue)
            {
                await _dbService.DeleteCandidate(CandidateId.Value);
                //await _dbService.AddActivityLog(ActivityLogType.Delete, User.Identity.Name, "Candidate Id :[" + CandidateId + "]");
            }

            return await OnGetAsync(RaceId);
        }

        public async Task<IActionResult> OnPostSaveCandidateAsync()
        {
            if (!ModelState.IsValid)
            {
                return await OnGetAsync(RaceId);
            }

            try
            {
                await _dbService.EditCandidate(Candidate);
                //await _dbService.AddActivityLog(ActivityLogType.Delete, User.Identity.Name, "Candidate Id :[" + Candidate.Id + "] ");

                return RedirectToPage("../Election/Index");
            }
            catch { }
            return Page();
        }
    }
}
