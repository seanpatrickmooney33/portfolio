using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpecialElection.Data;
using SpecialElection.Data.Model;
using SpecialElection.Models;

namespace SpecialElection.Areas.Special.Pages.ElectionView
{
    public class CreateModel : BasePageModel
    {
        [BindProperty]
        public InputModel Election { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(255)]
            [Display(Name = "Election Title")]
            public String Name { get; set; }

            [Required]
            [Display(Name = "Election Date")]
            public DateTime ElectionDate { get; set; }

            [Required]
            [Display(Name = "Is Active")]
            public Boolean IsActive { get; set; } = false;
        }

        public CreateModel(ApplicationDbService context) : base(context) { }

        public IActionResult OnGet()
        {
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
                await _dbService.CreateElection(new Election()
                {
                    Name = Election.Name,
                    ElectionDate = Election.ElectionDate.ToUniversalTime(),
                    IsActive = Election.IsActive,
                    CreatedBy = "Fixme",// need to set these to the logged in user info
                    ModifyBy = "Fixme", // need to set these to the logged in user info
                });

                return RedirectToPage("./Index");
            }
            catch
            {
                // log ex;
            }

            return Page();
        }
    }
}
