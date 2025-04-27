using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaekwondoRanking.Repository.Models
{
    [Table("AGEKL")]
    public partial class Agekl
    {
        public Agekl()
        {
            Categories = new HashSet<Category>();
            Subcmpt1s = new HashSet<Subcmpt1>();
        }

        [Key]
        [Column("IDAGEKL")]
        [StringLength(3)]
        public string Idagekl { get; set; } = null!;
        [Column("NAMEAGEKL")]
        [StringLength(255)]
        public string? Nameagekl { get; set; }

        [InverseProperty("AgeclassNavigation")]
        public virtual ICollection<Category> Categories { get; set; }
        [InverseProperty("AgeclassNavigation")]
        public virtual ICollection<Subcmpt1> Subcmpt1s { get; set; }
    }
}
