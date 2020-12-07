using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Prototype.Data;

namespace SpecialElection.Data.Model
{
    [Table(Common.DataBase.TABLE_CANDIDATE, Schema = Common.DataBase.SCHEMA_DBO)]
    public class Candidate : CandidateInfo
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Display Name")]
        [StringLength(255)]
        public new String DisplayName { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        [Required]
        public new PartyTypeEnum Party { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public new String FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Middle Name")]
        public new String MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public new String LastName { get; set; }


        [Required]
        [ForeignKey(Common.DataBase.TABLE_RACE)]
        public int RaceId { get; set; }
        public virtual Race Race { get; set; }

        public virtual ICollection<CandidateResult> CandidateResults { get; set; } = new List<CandidateResult>();


        public Candidate() { }
        public Candidate(Candidate c)
        {
            this.Id = c.Id;
            this.DisplayName = c.DisplayName;
            this.DisplayOrder = c.DisplayOrder;
            this.Party = c.Party;
            this.FirstName = c.FirstName;
            this.MiddleName = c.MiddleName;
            this.LastName = c.LastName;
            this.RaceId = c.RaceId;
        }
        

        public Candidate Clone() { return (Candidate)this.MemberwiseClone(); }

        public Boolean Compare(Candidate c)
        {
            return this.Id.Equals(c.Id)
                && this.DisplayName.Equals(c.DisplayName)
                && this.DisplayOrder.Equals(c.DisplayOrder)
                && this.Party.Equals(c.Party)
                && this.FirstName.Equals(c.FirstName)
                && this.MiddleName.Equals(c.MiddleName)
                && this.LastName.Equals(c.LastName)
                && this.RaceId.Equals(c.RaceId);
        }
    }

}
