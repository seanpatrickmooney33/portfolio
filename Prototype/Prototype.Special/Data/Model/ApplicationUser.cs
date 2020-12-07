using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpecialElection.Data.Model
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsAdmin { get; set; } = true;
    }
}
