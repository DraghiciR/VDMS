namespace VDMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserLogID { get; set; }

        public string UserID { get; set; }

        public string AffectedUserID { get; set; }

        [Required]
        [StringLength(1)]
        public string OperationType { get; set; }

        public DateTime LogDate { get; set; }

        //public virtual User User { get; set; }

        //public virtual User User1 { get; set; }
    }
}
