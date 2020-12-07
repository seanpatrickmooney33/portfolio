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
using SpecialElection.Models;

namespace SpecialElection.Areas.Special.Pages.ElectionView
{
    public class EditModel : BasePageModel
    {
        public EditModel(ApplicationDbService context) : base(context) { }

        [BindProperty]
        public InputModel Election { get; set; }

        public class InputModel : Election
        {
            public InputModel() { }
            public InputModel(Election e) : base(e) { }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Election e = await _dbService.GetElection().FirstOrDefaultAsync(m => m.Id == id);

            if (e == null)
            {
                return NotFound();
            }

            Election = new InputModel(e);

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
                await _dbService.EditElection(Election);
                //await _dbService.AddActivityLog(ActivityLogType.Edit, User.Identity.Name, "Edit Election. Id :["+Election.Id+"] Name : [" + Election.Name + "]");

                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElectionExists(Election.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool ElectionExists(int id)
        {
            return _dbService.GetElection().Any(e => e.Id == id);
        }
    }
}
