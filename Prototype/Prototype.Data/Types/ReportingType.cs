using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Prototype.Data
{
    public enum ReportingTypeEnum
    {
        Unknown,
        [Display(Name = "Election Night Report")] R,
        [Display(Name = "Semi-Final Election Night Report")] F,
        [Display(Name = "Update Report")]U,
        S
    }
}
