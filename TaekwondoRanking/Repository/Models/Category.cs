using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaekwondoRanking.Repository.Models
{
    [Table("CATEGORIES")]
    public partial class Category
    {
        public Category()
        {
            Subcmpt2s = new HashSet<Subcmpt2>();
        }

        [Key]
        [Column("IDCTGR")]
        [StringLength(12)]
        public string Idctgr { get; set; } = null!;
        [Column("AGECLASS")]
        [StringLength(3)]
        public string? Ageclass { get; set; }
        [Column("MF")]
        [StringLength(255)]
        public string? Mf { get; set; }
        [Column("NAMECTGR")]
        public string? Namectgr { get; set; }
        [Column("SSMA_TimeStamp")]
        public byte[] SsmaTimeStamp { get; set; } = null!;

        [ForeignKey("Ageclass")]
        [InverseProperty("Categories")]
        public virtual Agekl? AgeclassNavigation { get; set; }
        [InverseProperty("IdctgrNavigation")]
        public virtual ICollection<Subcmpt2> Subcmpt2s { get; set; }
    }
}
