using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Prototype.Data;
using Prototype.Lambda.API.Service;
using Prototype.Data.Models.VoteResults;
using Prototype.Data.Models.ViewModels;

namespace Prototype.Web.Areas.Static.Pages.Results
{
    public class CandidateVoteModel : PageModel
    {
        private readonly IApiService apiService;
        private readonly ILogger _logger;

        public CandidateVoteModel(ILogger<CandidateVoteModel> logger, IApiService a)
        {
            _logger = logger;
            apiService = a;
        }

        public VoteRecordViewModel VoteRecord { get; set; }
        public RedisKey Key { get; set; }

        public async Task<IActionResult> OnGetAsync(String id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            Key = new RedisKey(id);
            VoteRecord = apiService.GetVoteRecordViewModel(id);

            if (VoteRecord == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
