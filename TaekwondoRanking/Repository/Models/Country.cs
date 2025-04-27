using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaekwondoRanking.Repository.Models
{
    [Table("COUNTRY")]
    public partial class Country
    {
        public Country()
        {
            Athlets = new HashSet<Athlet>();
            Competitions = new HashSet<Competition>();
        }

        [Key]
        [Column("IDCNTR")]
        [StringLength(3)]
        public string Idcntr { get; set; } = null!;
        [Column("NAMECNTR")]
        public string? Namecntr { get; set; }
        [Column("SSMA_TimeStamp")]
        public byte[] SsmaTimeStamp { get; set; } = null!;

        [InverseProperty("CountryNavigation")]
        public virtual ICollection<Athlet> Athlets { get; set; }
        [InverseProperty("CountryNavigation")]
        public virtual ICollection<Competition> Competitions { get; set; }
    }
}
