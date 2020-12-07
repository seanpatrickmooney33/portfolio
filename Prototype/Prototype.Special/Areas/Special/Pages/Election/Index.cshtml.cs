using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SpecialElection.Data;
using SpecialElection.Data.Model;

namespace SpecialElection.Areas.Special.Pages.ElectionView
{
    public class IndexModel : BasePageModel
    {
        public IndexModel(ApplicationDbService context) : base(context) { }

        public List<Election> Election { get;set; }


        public async Task<IActionResult> OnGetAsync()
        {
            Election = await _dbService.GetElection()
                                               .Include(x => x.Races)
                                               .ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteElectionAsync(int ElectionId)
        {
            await _dbService.DeleteElection(ElectionId);
            return await OnGetAsync();
        }

        public async Task<IActionResult> OnPostDeleteRaceAsync(int RaceId)
        {
            await _dbService.DeleteRace(RaceId);
            return await OnGetAsync();
        }

        public async Task<IActionResult> OnPostActivateElectionAsync(int ElectionId)
        {
            await _dbService.ActivateElection(ElectionId);
            return await OnGetAsync();
        }

        public async Task<IActionResult> OnPostElectionSendAsync(int ElectionId)
        {
            return await OnGetAsync();
        }

        public async Task<IActionResult> OnPostElectionGetAsync(int ElectionId)
        {
            return await OnGetAsync();
        }

    }
}
