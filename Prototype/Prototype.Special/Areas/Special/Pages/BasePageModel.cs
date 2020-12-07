using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpecialElection.Data;

namespace SpecialElection.Areas.Special.Pages
{
    [Authorize]
    public abstract class BasePageModel : PageModel
    {
        protected readonly ApplicationDbService _dbService;
        protected BasePageModel(ApplicationDbService context)
        {
            _dbService = context;
        }
    }
}
