﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Prototype.Data;
using Prototype.Lambda.API.Service;
using Prototype.Data.Models.VoteResults;

namespace Prototype.Web.Areas.Static.Pages.Results
{
    public class JudgeModel : PageModel
    {
        private readonly IApiService apiService;
        private readonly ILogger _logger;

        public JudgeModel(ILogger<CandidateVoteModel> logger, IApiService a)
        {
            _logger = logger;
            apiService = a;
        }

        public VoteRecord<VoteResults<JudgeResult>> VoteRecord { get; set; }

        public async Task<IActionResult> OnGetAsync(String id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            VoteRecord = apiService.GetJudgeRecord(id);

            if (VoteRecord == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
