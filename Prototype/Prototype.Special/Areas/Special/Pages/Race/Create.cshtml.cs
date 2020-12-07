using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpecialElection.Data;
using SpecialElection.Data.Model;
using Prototype.Data;
using Microsoft.EntityFrameworkCore;

namespace SpecialElection.Areas.Special.Pages.RaceView
{
    public class CreateModel : BasePageModel
    {
        public CreateModel(ApplicationDbService context) : base(context) { }

        public String ElectionType { get; set; } = "Special Election";
        public String ElectionName { get; set; }

        public async Task<IActionResult> OnGetAsync(int? electionId)
        {
            if(electionId.HasValue == false)
            {
                return NotFound();
            }

            Election e = await _dbService.GetElection().Where(x => x.Id.Equals(electionId.Value)).FirstOrDefaultAsync();
            
            if(e == null)
            {
                return NotFound();
            }
            
            this.ElectionName = e.Name;
            this.Race = new Race() { ElectionId = e.Id };

            return Page();
        }


        [BindProperty]
        public Race Race { get; set; }

        
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _dbService.CreateRace(Race);
            return RedirectToPage("../Election/Index");
        }
    }
}
