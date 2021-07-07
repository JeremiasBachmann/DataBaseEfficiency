using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataBaseEfficiencyProjekt
{
    public partial class DatabaseConntext : DbContext
    {
        public DatabaseConntext()
            : base("name=DatabaseConntext")
        {
        }

        public virtual DbSet<tblAuthors> tblAuthors { get; set; }
        public virtual DbSet<tblBooks> tblBooks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblAuthors>()
                .HasMany(e => e.tblBooks)
                .WithOptional(e => e.tblAuthors)
                .HasForeignKey(e => e.Auhthor_id);
        }
    }
}
