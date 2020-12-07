using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SpecialElection.Data;
using SpecialElection.Data.Model;

namespace SpecialElection.Areas.Special.Pages.PrecinctView
{
    public class EditRacePrecinctsModel : BasePageModel
    {
        public EditRacePrecinctsModel(ApplicationDbService context) : base(context) { }

        [BindProperty]
        public List<InputModel> RaceCountyData { get;set; }
        
        public class InputModel : RaceCountyData
        {
            public InputModel() { }
            public InputModel(RaceCountyData r) : base(r) { }
        }

        public Race Race { get; set; }


        public async Task<IActionResult> OnGetAsync(int? RaceId)
        {
            if (RaceId == null)
            {
                return NotFound();
            }

            Race = await _dbService.GetRaces().Include(x => x.RaceCountyDataList).FirstOrDefaultAsync(x => x.Id.Equals(RaceId.Value));

            if (Race == null)
            {
                return NotFound();
            }

            RaceCountyData = Race.RaceCountyDataList.Select(x => new InputModel(x)).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            try
            {
                List<int> Ids = this.RaceCountyData.Select(x => x.Id).ToList();
                List<RaceCountyData> temp = await _dbService.GetRaceCountyData().Where(x => Ids.Contains(x.Id)).OrderBy(x => x.Id).ToListAsync();

                this.RaceCountyData.ForEach(x => {
                    RaceCountyData z = temp.FirstOrDefault(y => y.Id.Equals(x.Id));
                    if (z != null) z.NumberOfPrecinct = x.NumberOfPrecinct;
                });
                await _dbService.EditRaceCountyData(temp);

                return RedirectToPage("../Election/Index");
            }
            catch { }

            return Page();
        }
    }
}
