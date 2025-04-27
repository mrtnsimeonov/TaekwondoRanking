using System;
using System.Collections.Generic;

namespace TaekwondoRanking.Models
{
    public partial class Competition
    {
        public Competition()
        {
            Subcmpt1s = new HashSet<Subcmpt1>();
        }

        public int Idcmpt { get; set; }
        public string? Country { get; set; }
        public string? Namecmpt { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Tilldate { get; set; }
        public string? Rangelabel { get; set; }
        public byte[] SsmaTimeStamp { get; set; } = null!;

        public virtual Country? CountryNavigation { get; set; }
        public virtual ICollection<Subcmpt1> Subcmpt1s { get; set; }
    }
}
