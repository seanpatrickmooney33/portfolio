using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Data
{
    public enum ElectionTypeEnum
    {
        // PP Presidential Primary / Statewide Election
        [Display(Name = "Statewide Election",               ShortName = "PP")] Presidential = 0,
        // DP Direct Primary
        [Display(Name = "Direct Primary Election",          ShortName = "DP")] DirectPrimary = 3,
        // GP Gov Primary
        [Display(Name = "Governor Primary Election",        ShortName = "GP")] GovernorPrimary = 4,
        // PG Presidential general
        [Display(Name = "Presidential General Election",    ShortName = "PG")] PresidentialGeneral = 5,
        // GG Gov General
        [Display(Name = "Governor General Election",        ShortName = "GG")] GovernorGeneral = 6,

        // RE District Recall
        [Display(Name = "Recall Election",                  ShortName = "RE")] Recall = 1,
        // RS Statewide Recall
        [Display(Name = "Statewide Recall Election",        ShortName = "RS")] StatewideRecall = 7,

        // SE District Special
        [Display(Name = "Special Election",                 ShortName = "SE")] Special = 2,
        // SS Statewide Special
        [Display(Name = "Statewide Special Election",       ShortName = "SS")] StatwideSpecial = 8,
    }
}


/*
 General Election       (cv2 / cr78)
 Primary                (cv2 / cr78)
 Special                (Hanlder)
 Statewide Recall       (cv2 / cr78)
 District Recall        (Hanlder)
 */


/*
  
    XX00
    // PP Presidential Primary / Statewide Election
    // DP Direct Primary
    // GP Gov Primary
    // PG Presidential general
    // GG Gov General

    XX01
    // RE District Recall
    // SS Statewide Special -> RS Statewide Recall 

    XX02
    // SE District Special

 */
