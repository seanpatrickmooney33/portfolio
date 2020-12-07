using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpecialElection.Data.Model
{
    [Table(Common.DataBase.TABLE_ELECTION, Schema = Common.DataBase.SCHEMA_DBO)]
    public class Election
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Election Title")]
        public String Name { get; set; }

        [Required]
        [Display(Name = "Election Date")]
        public DateTime ElectionDate { get; set; }

        [Required]
        [Display(Name = "Is Active")]
        public Boolean IsActive { get; set; } = false;

        [Required]
        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        [Display(Name = "Created By")]
        public String CreatedBy { get; set; }

        [Required]
        [Display(Name = "Modify Date")]
        public DateTime ModifyDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        [Display(Name = "Modified By")]
        public String ModifyBy { get; set; }

        public virtual ICollection<Race> Races { get; set; } = new List<Race>();

        public Election() { }
        public Election(Election e)
        {
            this.Id = e.Id;
            this.Name = e.Name;
            this.ElectionDate = e.ElectionDate;
            this.IsActive = e.IsActive;
            this.CreateDate = e.CreateDate;
            this.CreatedBy = e.CreatedBy;
            this.ModifyDate = e.ModifyDate;
            this.ModifyBy = e.ModifyBy;
        }
        public Election Clone() { return (Election)this.MemberwiseClone(); }

        public Boolean Compare(Election e)
        {
            return
            this.Id.Equals(e.Id)
            && this.Name.Equals(e.Name)
            && this.ElectionDate.Equals(e.ElectionDate)
            && this.IsActive.Equals(e.IsActive)
            && this.CreateDate.Equals(e.CreateDate)
            && this.CreatedBy.Equals(e.CreatedBy)
            && this.ModifyDate.Equals(e.ModifyDate)
            && this.ModifyBy.Equals(e.ModifyBy);
        }
    }
}