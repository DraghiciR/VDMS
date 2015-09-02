namespace VDMS.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class VDMSModel : DbContext
    {
        public VDMSModel()
            : base("name=VDMSConnectionString")
        {
        }

        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<DocumentLog> DocumentLogs { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<DocumentType> DocumentTypes { get; set; }
        //public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<UserLog> UserLogs { get; set; }
        //public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Branch>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Branch>()
                .HasMany(e => e.Documents)
                .WithRequired(e => e.Branch)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DocumentLog>()
                .Property(e => e.OperationType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Document>()
                .Property(e => e.DocSerial)
                .IsUnicode(false);

            modelBuilder.Entity<Document>()
                .Property(e => e.Recipient)
                .IsUnicode(false);

            modelBuilder.Entity<Document>()
                .Property(e => e.Description)
                .IsUnicode(false);

            //modelBuilder.Entity<Document>()
            //    .HasMany(e => e.DocumentLogs)
            //    .WithRequired(e => e.Document)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<DocumentType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<DocumentType>()
                .Property(e => e.Serial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DocumentType>()
                .HasMany(e => e.Documents)
                .WithRequired(e => e.DocumentType)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Role>()
            //    .Property(e => e.Description)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Role>()
            //    .HasMany(e => e.Users)
            //    .WithRequired(e => e.Role)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserLog>()
                .Property(e => e.OperationType)
                .IsFixedLength()
                .IsUnicode(false);

            //modelBuilder.Entity<User>()
            //    .Property(e => e.Username)
            //    .IsUnicode(false);

            //modelBuilder.Entity<User>()
            //    .Property(e => e.Password)
            //    .IsUnicode(false);

            //modelBuilder.Entity<User>()
            //    .Property(e => e.Email)
            //    .IsUnicode(false);

            //modelBuilder.Entity<User>()
            //    .HasMany(e => e.DocumentLogs)
            //    .WithRequired(e => e.User)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<User>()
            //    .HasMany(e => e.Documents)
            //    .WithRequired(e => e.User)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<User>()
            //    .HasMany(e => e.UserLogs)
            //    .WithRequired(e => e.User)
            //    .HasForeignKey(e => e.AffectedUserID)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<User>()
            //    .HasMany(e => e.UserLogs1)
            //    .WithRequired(e => e.User1)
            //    .HasForeignKey(e => e.UserID)
            //    .WillCascadeOnDelete(false);
        }
    }
}
