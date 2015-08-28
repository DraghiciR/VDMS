namespace VDMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Document
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Document()
        {
            DocumentLogs = new HashSet<DocumentLog>();
        }

        [Key]
        public int DocID { get; set; }

        [Required]
        [StringLength(20)]
        public string DocSerial { get; set; }

        public int DocTypeID { get; set; }

        public int BranchID { get; set; }

        public string UserID { get; set; }

        public bool Inbound { get; set; }

        [StringLength(100)]
        public string Recipient { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual Branch Branch { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentLog> DocumentLogs { get; set; }

        public virtual DocumentType DocumentType { get; set; }

        //public virtual User User { get; set; }
    }
}
