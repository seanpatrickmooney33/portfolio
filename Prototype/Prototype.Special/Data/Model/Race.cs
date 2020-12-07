using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Prototype.Data;
using System.Linq;

namespace SpecialElection.Data.Model
{
    [Table(Common.DataBase.TABLE_RACE, Schema = Common.DataBase.SCHEMA_DBO)]
    public class Race
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Race Name")]
        [StringLength(255)]
        public String Name { get; set; }

        [Required]
        [Display(Name = "Race Type")]
        public RaceTypeEnum Type { get; set; }

        [Required]
        [Range(1, 80)]
        public int District { get; set; }

        [StringLength(255)]
        public String Description { get; set; }

        public Boolean Locked { get; set; } = false;

        [Required]
        [ForeignKey(Common.DataBase.TABLE_ELECTION)]
        public int ElectionId { get; set; }
        public virtual Election Election { get; set; }

        private IEnumerable<Candidate> _candidates = new List<Candidate>();
        public virtual IEnumerable<Candidate> Candidates {
            get { return this._candidates.OrderBy(x => x.DisplayOrder); } 
            set { _candidates = value; } 
        } 
        public virtual IEnumerable<RaceCountyData> RaceCountyDataList { get; set; } = new List<RaceCountyData>();

        public Race() { }
        public Race(Race r)
        {
            this.Id = r.Id;
            this.Name = r.Name;
            this.Type = r.Type;
            this.District = r.District;
            this.Description = r.Description;
            this.Locked = r.Locked;
            this.ElectionId = r.ElectionId;
        }

        public Race Clone() { return (Race)this.MemberwiseClone(); }
        
        public Boolean Compare(Race r) {
            return
            this.Id.Equals(r.Id)
            && this.Name.Equals(r.Name)
            && this.Type.Equals(r.Type)
            && this.District.Equals(r.District)
            && this.Description.Equals(r.Description)
            && this.Locked.Equals(r.Locked)
            && this.ElectionId.Equals(r.ElectionId);
        }
    }
}