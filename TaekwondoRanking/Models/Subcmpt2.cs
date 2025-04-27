using System;
using System.Collections.Generic;

namespace TaekwondoRanking.Models
{
    public partial class Subcmpt2
    {
        public Subcmpt2()
        {
            Results = new HashSet<Result>();
        }

        public int Idsubcmpt2 { get; set; }
        public int Idsubcmpt1 { get; set; }
        public string? Idctgr { get; set; }

        public virtual Category? IdctgrNavigation { get; set; }
        public virtual Subcmpt1 Idsubcmpt1Navigation { get; set; } = null!;
        public virtual ICollection<Result> Results { get; set; }
    }
}
