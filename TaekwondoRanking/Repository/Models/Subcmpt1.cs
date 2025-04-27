using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaekwondoRanking.Repository.Models
{
    [Table("SUBCMPT1")]
    [Index("Idcmpt", Name = "SUBCMPT1$IDCMPT")]
    [Index("Idsubcmpt1", Name = "SUBCMPT1$IDSUBCMPT1")]
    public partial class Subcmpt1
    {
        public Subcmpt1()
        {
            Subcmpt2s = new HashSet<Subcmpt2>();
        }

        [Key]
        [Column("IDSUBCMPT1")]
        public int Idsubcmpt1 { get; set; }
        [Column("IDCMPT")]
        public int? Idcmpt { get; set; }
        [Column("PLAYDATE")]
        [Precision(0)]
        public DateTime? Playdate { get; set; }
        [Column("AGECLASS")]
        [StringLength(3)]
        public string Ageclass { get; set; } = null!;
        [Column("RANK")]
        [StringLength(3)]
        public string? Rank { get; set; }

        [ForeignKey("Ageclass")]
        [InverseProperty("Subcmpt1s")]
        public virtual Agekl AgeclassNavigation { get; set; } = null!;
        [ForeignKey("Idcmpt")]
        [InverseProperty("Subcmpt1s")]
        public virtual Competition? IdcmptNavigation { get; set; }
        [InverseProperty("Idsubcmpt1Navigation")]
        public virtual ICollection<Subcmpt2> Subcmpt2s { get; set; }
    }
}
