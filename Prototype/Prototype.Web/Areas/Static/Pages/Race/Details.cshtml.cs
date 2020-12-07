using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Prototype.Data;
using Prototype.Lambda.API.Service;

namespace Prototype.Web.Areas.Static.Pages.Race
{
    public class DetailsModel : PageModel
    {
        private readonly IApiService apiService;
        private readonly ILogger _logger;

        public DetailsModel(ILogger<DetailsModel> logger, IApiService a)
        {
            _logger = logger;
            apiService = a;
        }

        public RaceInfo RaceInfo { get; set; }

        public async Task<IActionResult> OnGetAsync(String id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            RaceInfo = new RaceInfo(apiService.GetRaceInfo(id));

            if (RaceInfo == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
