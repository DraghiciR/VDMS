namespace VDMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DocumentType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DocumentType()
        {
            Documents = new HashSet<Document>();
        }

        [Key]
        [Display(Name = "Document Type")]
        public int DocTypeID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Type", AutoGenerateFilter = true)]
        public string Name { get; set; }

        [Required]
        [StringLength(3)]
        [Display(Name = "Serial")]
        public string Serial { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Document> Documents { get; set; }
    }
}
