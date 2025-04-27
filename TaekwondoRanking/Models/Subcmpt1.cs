using System;
using System.Collections.Generic;

namespace TaekwondoRanking.Models
{
    public partial class Subcmpt1
    {
        public Subcmpt1()
        {
            Subcmpt2s = new HashSet<Subcmpt2>();
        }

        public int Idsubcmpt1 { get; set; }
        public int? Idcmpt { get; set; }
        public DateTime? Playdate { get; set; }
        public string Ageclass { get; set; } = null!;
        public string? Rank { get; set; }

        public virtual Agekl AgeclassNavigation { get; set; } = null!;
        public virtual Competition? IdcmptNavigation { get; set; }
        public virtual ICollection<Subcmpt2> Subcmpt2s { get; set; }
    }
}
