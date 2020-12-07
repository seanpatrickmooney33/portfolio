using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SpecialElection.Data;
using SpecialElection.Data.Model;
using SpecialElection.Service;
using System.IO;
using System.Reflection;

namespace SpecialElection.Areas.Special.Pages
{
    public class IndexModel : BasePageModel
    {
        private readonly MessageService messageService;
        private readonly String assemblyLocation;

        public IndexModel(ApplicationDbService context, MessageService m) : base(context)
        {
            messageService = m;
            assemblyLocation = Assembly.GetExecutingAssembly().Location;
            assemblyLocation = assemblyLocation.Remove(assemblyLocation.LastIndexOf(@"\"));
            assemblyLocation = assemblyLocation.Remove(assemblyLocation.LastIndexOf(@"\"));
        }

        public Election Election { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Election = await _dbService.GetElection()
                                        .Where(x => x.IsActive)
                                        .Include(x => x.Races)
                                        .ThenInclude(x => x.RaceCountyDataList)
                                        .FirstOrDefaultAsync();

            if(Election == null)
            {
                return RedirectToPage("../Election/Index");
            }

            return Page();
        }

        private String getFileName(String prefix)
        {
            DateTime dt = DateTime.UtcNow;
            
            // need to add election type to data structure
            //Election.Type
            return assemblyLocation + @"\" + prefix + dt.ToString("yy") + "SE" + ".txt";
        }

        public async Task<IActionResult> OnPostAddRandomResultsAsync() 
        {
            await _dbService.AddRandomResults();
            return RedirectToPage("../Result/Index");
        }

        // set the result for all candadtes to 0
        public async Task<IActionResult> OnPostAddZeroResultsAsync() 
        {
            await _dbService.AddZeroResult();
            return RedirectToPage("../Result/Index");
        }
        
        public async Task<IActionResult> OnPostGenerateRMSGAsync()
        {
            String result = await messageService.GenerateRMSG();
            System.IO.File.WriteAllText(getFileName("R"), result);
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostGenerateCMSGAsync()
        {
            String result = await messageService.GenerateCMSG();
            System.IO.File.WriteAllText(getFileName("C"), result);
            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnPostGeneratePMSGAsync()
        {
            String result = await messageService.GeneratePMSG();
            System.IO.File.WriteAllText(getFileName("P"), result);
            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnPostGenerateSMSGAsync()
        {
            String result = messageService.GenerateSMSG();
            System.IO.File.WriteAllText(getFileName("S"), result);
            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnPostGenerateVMSGAsync()
        {
            String result = await messageService.GenerateVMSG();
            System.IO.File.WriteAllText(getFileName("V"), result);
            // need to post the results to server
            return RedirectToPage("Index");
        }

    }
}