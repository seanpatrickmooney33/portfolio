using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpecialElection.Data;
using Prototype.Data;
using SpecialElection.Data.Model;

namespace SpecialElection.Areas.Special.Pages.RaceView
{
    public class EditModel : BasePageModel
    {
        public EditModel(ApplicationDbService context) : base(context) { }

        public String ElectionType { get; set; } = "Special Election";

        public Election Election { get; set; }

        [BindProperty]
        public InputModel Race { get; set; }
        
        public class InputModel : Race
        {
            public InputModel() { }
            public InputModel(Race r) : base(r) { }
        }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Race r = await _dbService.GetRaces().Include(x => x.Election)
                                                .FirstOrDefaultAsync(m => m.Id == id);

            if (r == null)
            {
                return NotFound();
            }

            this.Race = new InputModel(r);
            this.Election = r.Election;

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
                await _dbService.EditRace(Race);

                return RedirectToPage("../Election/Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RaceExists(Race.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        private bool RaceExists(int id)
        {
            return _dbService.GetRaces().Any(e => e.Id == id);
        }
    }
}
