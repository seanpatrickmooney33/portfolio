using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Prototype.Data;
using Prototype.Lambda.API.Service;

namespace Prototype.Web.Areas.Static.Pages.Candidate
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

        public CandidateInfo Candidate { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Candidate = apiService.GetCandidate(id.Value);

            if (Candidate == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
