namespace DataBaseEfficiencyProjekt
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblAuthors
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblAuthors()
        {
            tblBooks = new HashSet<tblBooks>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Author_name { get; set; }

        [StringLength(50)]
        public string country { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBooks> tblBooks { get; set; }
    }
}
