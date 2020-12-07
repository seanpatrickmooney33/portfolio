using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpecialElection.Data;
using SpecialElection.Data.Model;

namespace SpecialElection.Areas.Special.Pages.CandidateView
{
    public class CreateModel : BasePageModel
    {
        [BindProperty]
        public Candidate Candidate { get; set; }


        public CreateModel(ApplicationDbService context) : base(context) { }

        public async Task<IActionResult> OnGetAsync(int? RaceId)
        {
            if(RaceId == null)
            {
                return NotFound();
            }

            Race r = await _dbService.GetRaces().Where(x => x.Id.Equals(RaceId.Value)).FirstOrDefaultAsync();

            if(r == null)
            {
                return NotFound();
            }

            this.Candidate = new Candidate() { RaceId = r.Id };
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
                await _dbService.CreateCandidate(Candidate);
                return RedirectToPage("./Index", new { Candidate.RaceId });
            }
            catch { }

            return Page();
        }
    }
}
