using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaekwondoRanking.Repository.Models
{
    [Table("COMPETITIONS")]
    [Index("Country", Name = "COMPETITIONS$IDCOUNT")]
    public partial class Competition
    {
        public Competition()
        {
            Subcmpt1s = new HashSet<Subcmpt1>();
        }

        [Key]
        [Column("IDCMPT")]
        public int Idcmpt { get; set; }
        [Column("COUNTRY")]
        [StringLength(3)]
        public string? Country { get; set; }
        [Column("NAMECMPT")]
        public string? Namecmpt { get; set; }
        [Column("FROMDATE")]
        [Precision(0)]
        public DateTime? Fromdate { get; set; }
        [Column("TILLDATE")]
        [Precision(0)]
        public DateTime? Tilldate { get; set; }
        [Column("RANGELABEL")]
        public string? Rangelabel { get; set; }
        [Column("SSMA_TimeStamp")]
        public byte[] SsmaTimeStamp { get; set; } = null!;

        [ForeignKey("Country")]
        [InverseProperty("Competitions")]
        public virtual Country? CountryNavigation { get; set; }
        [InverseProperty("IdcmptNavigation")]
        public virtual ICollection<Subcmpt1> Subcmpt1s { get; set; }
    }
}
