using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Prototype.Data;

namespace SpecialElection.Data.Model
{
    public enum ActivityLogType
    {
        Create = 0,
        Edit = 1,
        Delete = 2,
        Post = 3,
    }

    [Table(Common.DataBase.TABLE_ACTIVITYLOG, Schema = Common.DataBase.SCHEMA_DBO)]
    public class ActivityLog
    {
        [Required][Key]
        public Guid ActivityLogId { get; set; }

        [Required]
        public ActivityLogType ActivityLogType { get; set; }

        [Required]
        public String Description { get; set; }
        [Required]
        public String ActiveUserName { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
    }
}
