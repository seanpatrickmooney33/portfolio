using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Prototype.Data;

namespace SpecialElection.Data.Model
{
    [Table(Common.DataBase.TABLE_RACESCOUNTYDATA, Schema = Common.DataBase.SCHEMA_DBO)]
    public class RaceCountyData
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(Common.DataBase.TABLE_RACE)]
        public int RaceId { get; set; }
        public virtual Race Race { get; set; }

        [Required]
        public CountyTypeEnum County { get; set; }

        [Display(Name = "Number of Precinct")]
        [Range(0, int.MaxValue)]
        public int NumberOfPrecinct { get; set; } = 0;

        [Display(Name = "Precincts Reporting")]
        public int PrecinctsReporting { get; set; } = 0;

        public DateTime UpdatedDateTime { get; set; } = DateTime.UtcNow;

        public virtual ICollection<CandidateResult> CandidateResults { get; set; } = new List<CandidateResult>();

        public RaceCountyData() { }
        public RaceCountyData(RaceCountyData r)
        {
            this.Id = r.Id;
            this.RaceId = r.RaceId;
            this.County = r.County;
            this.NumberOfPrecinct = r.NumberOfPrecinct;
            this.PrecinctsReporting = r.PrecinctsReporting;
            this.UpdatedDateTime = r.UpdatedDateTime;
        }

        public RaceCountyData Clone() { return (RaceCountyData)this.MemberwiseClone(); }

        public Boolean Compare(RaceCountyData r)
        {
            return this.Id.Equals(r.Id)
                && this.RaceId.Equals(r.RaceId)
                && this.County.Equals(r.County)
                && this.NumberOfPrecinct.Equals(r.NumberOfPrecinct)
                && this.PrecinctsReporting.Equals(r.PrecinctsReporting)
                && this.UpdatedDateTime.Equals(r.UpdatedDateTime);
        }
    }
}