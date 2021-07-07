namespace DataBaseEfficiencyProjekt
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblBooks
    {
        public int Id { get; set; }

        public int? Auhthor_id { get; set; }

        public int? Price { get; set; }

        public int? Edition { get; set; }

        public virtual tblAuthors tblAuthors { get; set; }
    }
}
