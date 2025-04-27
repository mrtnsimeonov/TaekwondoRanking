using System;
using System.Collections.Generic;

namespace TaekwondoRanking.Models
{
    public partial class Result
    {
        public string Idathlet { get; set; } = null!;
        public int Idsubcmpt2 { get; set; }
        public int? Place { get; set; }
        public double? Points { get; set; }
        public byte[] SsmaTimeStamp { get; set; } = null!;

        public virtual Athlet IdathletNavigation { get; set; } = null!;
        public virtual Subcmpt2 Idsubcmpt2Navigation { get; set; } = null!;
    }
}
