using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaekwondoRanking.Repository.Models
{
    [Table("SUBCMPT2")]
    [Index("Idsubcmpt1", Name = "SUBCMPT2$IDSUBCMPT1")]
    public partial class Subcmpt2
    {
        public Subcmpt2()
        {
            Results = new HashSet<Result>();
        }

        [Key]
        [Column("IDSUBCMPT2")]
        public int Idsubcmpt2 { get; set; }
        [Column("IDSUBCMPT1")]
        public int Idsubcmpt1 { get; set; }
        [Column("IDCTGR")]
        [StringLength(12)]
        public string? Idctgr { get; set; }

        [ForeignKey("Idctgr")]
        [InverseProperty("Subcmpt2s")]
        public virtual Category? IdctgrNavigation { get; set; }
        [ForeignKey("Idsubcmpt1")]
        [InverseProperty("Subcmpt2s")]
        public virtual Subcmpt1 Idsubcmpt1Navigation { get; set; } = null!;
        [InverseProperty("Idsubcmpt2Navigation")]
        public virtual ICollection<Result> Results { get; set; }
    }
}
