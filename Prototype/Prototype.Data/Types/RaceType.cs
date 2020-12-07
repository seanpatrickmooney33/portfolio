using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Prototype.Data
{
    public enum RaceTypeEnum
    {
        [Display(Name = "President", ShortName = "01")] President = 1,
        [Display(Name = "Us Governor", ShortName = "02")] Governor = 2,
        [Display(Name = "Lieutenant Governor", ShortName = "03")] LieutenantGovernor = 3,
        [Display(Name = "Secretary Of State", ShortName = "04")] SecretaryOfState = 4,
        [Display(Name = "Controller", ShortName = "05")] Controller = 5,
        [Display(Name = "Treasurer", ShortName = "06")] Treasurer = 6,
        [Display(Name = "Attorney General", ShortName = "07")] AttorneyGeneral = 7,
        [Display(Name = "Insurance Commissioner", ShortName = "08")] InsuranceCommissioner = 8,
        [Display(Name = "Board Of Equalization", ShortName = "09")] BoardOfEqualization = 9,
        [Display(Name = "Us Senate", ShortName = "10")] UsSenate = 10,
        [Display(Name = "Us Representative", ShortName = "11")] Usrep = 11,
        [Display(Name = "State Senate", ShortName = "12")] StateSenate = 12,
        [Display(Name = "State Assembly", ShortName = "13")] StateAssembly = 13,
        [Display(Name = "Supreme Court", ShortName = "14")] SupremeCourt = 14,
        [Display(Name = "Courts Of Appeal", ShortName = "15")] CourtsOfAppeal = 15,
        [Display(Name = "Superintendent Of Public Instruction", ShortName = "16")] SuperIntendentOfPublicInstruction = 16,
        [Display(Name = "Ballot Measures", ShortName = "19")] BallotMeasures = 19
    }
}
