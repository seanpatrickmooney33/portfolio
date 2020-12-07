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
    public class IndexModel : PageModel
    {
        private readonly IApiService apiService;
        private readonly ILogger _logger;

        public IndexModel(ILogger<IndexModel> logger, IApiService a)
        {
            _logger = logger;
            apiService = a;
        }

        public List<CandidateInfo> Candidates { get;set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Candidates = apiService.GetCandidate().ToList();
            return Page();
        }
    }
}
