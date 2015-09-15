namespace VDMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Document
    {
        [Key]
        public int DocID { get; set; }

        // [Required]
        [StringLength(20)]
        [Display(Name = "Document Serial")]
        public string DocSerial { get; set; }

        [Display(Name = "Type")]
        [ForeignKey("DocumentType")]
        public int DocTypeID { get; set; }

        [Display(Name = "Branch")]
        public int BranchID { get; set; }

        [Display(Name = "UserID")]
        public string UserID { get; set; }

        [Display(Name = "Inbound")]
        public bool Inbound { get; set; }

        [StringLength(100)]
        [Display(Name = "Recipient")]
        public string Recipient { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Disabled")]
        public bool Disabled { get; set; }

        [Display(Name = "Disable Date")]
        public DateTime? DisabledDate { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual DocumentType DocumentType { get; set; }

        [NotMapped]
        public string UserName { get; set; }
    }
}
