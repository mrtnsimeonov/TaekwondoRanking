using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaekwondoRanking.Repository.Models
{
    [Table("RESULTS")]
    [Index("Idathlet", Name = "RESULTS$IDATHLET")]
    public partial class Result
    {
        [Key]
        [Column("IDATHLET")]
        [StringLength(10)]
        public string Idathlet { get; set; } = null!;
        [Key]
        [Column("IDSUBCMPT2")]
        public int Idsubcmpt2 { get; set; }
        [Column("PLACE")]
        public int? Place { get; set; }
        [Column("POINTS")]
        public double? Points { get; set; }
        [Column("SSMA_TimeStamp")]
        public byte[] SsmaTimeStamp { get; set; } = null!;

        [ForeignKey("Idathlet")]
        [InverseProperty("Results")]
        public virtual Athlet IdathletNavigation { get; set; } = null!;
        [ForeignKey("Idsubcmpt2")]
        [InverseProperty("Results")]
        public virtual Subcmpt2 Idsubcmpt2Navigation { get; set; } = null!;
    }
}
