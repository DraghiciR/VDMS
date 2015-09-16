namespace VDMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserLog
    {
        [Key]
        public int UserLogID { get; set; }

        public string UserID { get; set; }

        public string AffectedUserID { get; set; }

        [Required]
        [StringLength(30)]
        public string OperationType { get; set; }

        public DateTime LogDate { get; set; }

        [NotMapped]
        public string UserName { get; set; }
        [NotMapped]
        public string AffectedUserName { get; set; }
    }
}
