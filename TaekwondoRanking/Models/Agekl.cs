using System;
using System.Collections.Generic;

namespace TaekwondoRanking.Models
{
    public partial class Agekl
    {
        public Agekl()
        {
            Categories = new HashSet<Category>();
            Subcmpt1s = new HashSet<Subcmpt1>();
        }

        public string Idagekl { get; set; } = null!;
        public string? Nameagekl { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Subcmpt1> Subcmpt1s { get; set; }
    }
}
