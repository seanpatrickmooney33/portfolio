using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpecialElection.Data.Model
{
    [Table(Common.DataBase.TABLE_CANDIDATERESULT, Schema = Common.DataBase.SCHEMA_DBO)]
    public class CandidateResult
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(Common.DataBase.TABLE_RACESCOUNTYDATA)]
        public int RaceCountyDataId { get; set; }
        public virtual RaceCountyData RaceCountyData { get; set; }

        [Required]
        [ForeignKey(Common.DataBase.TABLE_CANDIDATE)]
        public int CandidateId { get; set; }
        public virtual Candidate Candidate { get; set; }

        [Required]
        public int Votes { get; set; } = 0;

        [Required]
        public double Percent { get; set; } = 0.0;

        public CandidateResult() { }
        public CandidateResult(CandidateResult c)
        {
            this.Id = c.Id;
            this.RaceCountyDataId = c.RaceCountyDataId;
            this.CandidateId = c.CandidateId;
            this.Votes = c.Votes;
            this.Percent = c.Percent;
        }

        public CandidateResult Clone() { return (CandidateResult)this.MemberwiseClone(); }

        public Boolean Compare(CandidateResult candidateResult)
        {
            return (this.Id.Equals(candidateResult.Id)
            && this.RaceCountyDataId.Equals(candidateResult.RaceCountyDataId)
            && this.CandidateId.Equals(candidateResult.CandidateId)
            && this.Votes.Equals(candidateResult.Votes)
            && this.Percent.Equals(candidateResult.Percent));
        }
    }
}