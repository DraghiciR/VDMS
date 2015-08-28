namespace VDMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DocumentLog
    {
        [Key]
        public int LogID { get; set; }

        public int DocID { get; set; }

        public string UserID { get; set; }

        [Required]
        [StringLength(1)]
        public string OperationType { get; set; }

        public DateTime LogDate { get; set; }

        public virtual Document Document { get; set; }

        //public virtual User User { get; set; }
    }
}
