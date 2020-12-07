using Prototype.Data.Models.VoteResults;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Prototype.Data.Models.ViewModels
{
    public class VoteResultViewModel
    {
        [Display(Name = "Candidate Display Name")]
        public String CandidateDisplayName { get; set; }
        public PartyTypeEnum CandidateParty { get; set; }

        [Display(Name = "Votes")]
        public int? LocalVote { get; set; }
        [Display(Name = "Party Percent")]
        public double? LocalPartyPercent { get; set; }
        [Display(Name = "Race Percent")]
        public double? LocalRacePercent { get; set; }

        [Display(Name = "Votes")]
        public int? GreaterAreaVote { get; set; }
        [Display(Name = "Party Percent")]
        public double? GreaterAreaPartyPercent { get; set; }
        [Display(Name = "Race Percent")]
        public double? GreaterAreaRacePercent { get; set; }

    }

    public class VoteRecordViewModel
    {
        // might not need key in this datastrutures
        public RedisKey Key { get; set; }

        [Display(Name = "Race Name")]
        public String RaceName { get; set; }
        public List<VoteResultViewModel> VoteResults { get; set; } = new List<VoteResultViewModel>();
        public VoteRecordViewModel() { }
    }
}
