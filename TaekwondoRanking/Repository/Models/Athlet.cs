using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaekwondoRanking.Repository.Models
{
    [Table("ATHLETS")]
    public partial class Athlet
    {
        public Athlet()
        {
            Results = new HashSet<Result>();
        }

        [Key]
        [Column("IDATHLET")]
        [StringLength(10)]
        public string Idathlet { get; set; } = null!;
        [Column("COUNTRY")]
        [StringLength(3)]
        public string? Country { get; set; }
        [Column("NAME")]
        public string? Name { get; set; }
        [Column("SSMA_TimeStamp")]
        public byte[] SsmaTimeStamp { get; set; } = null!;

        [ForeignKey("Country")]
        [InverseProperty("Athlets")]
        public virtual Country? CountryNavigation { get; set; }
        [InverseProperty("IdathletNavigation")]
        public virtual ICollection<Result> Results { get; set; }
    }
}
