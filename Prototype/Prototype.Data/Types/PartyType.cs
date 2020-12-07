using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Prototype.Data
{
    public enum PartyTypeEnum
    {
        [Display( Name = "Not a party race", ShortName = "Not a party race")] NotAPartyRace = 0,
        [Display( Name = "Democratic", ShortName = "Dem")] Democratic = 1,
        [Display( Name = "Republican", ShortName = "Rep")] Republican = 2,
        [Display( Name = "American Independent", ShortName = "AI")] AmericanIndependent = 3,
        [Display( Name = "Green", ShortName =  "Grn")] Green = 4,
        [Display( Name = "Libertarian", ShortName =  "Lib")] Libertarian = 5,
        [Display( Name = "Natural Law", ShortName =  "NL")] NaturalLaw = 6,
        [Display( Name = "Reform", ShortName =  "Ref")] Reform = 7,
        [Display( Name = "Peace and Freedom", ShortName =  "P&F")] PeaceandFreedom = 8,
        [Display( Name = "Independent", ShortName =  "Ind")] Independent = 9,
        [Display( Name = "Dem/Rep", ShortName =  "Dem/Rep")] DemRep = 10,
        [Display( Name = "RepDem", ShortName =  "RepDem")] RepDem = 11,
        // intentionally skip 12 for whatever reason that has been lost in time
        [Display( Name = "Non-Partisan", ShortName =  "Non")] NonPartisan = 13,
        [Display( Name = "No Party Preference", ShortName =  "NPP")] NoPartyPreference = 14,
    }

}
