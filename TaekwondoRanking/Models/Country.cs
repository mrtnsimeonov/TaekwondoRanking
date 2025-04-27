using System;
using System.Collections.Generic;

namespace TaekwondoRanking.Models
{
    public partial class Country
    {
        public Country()
        {
            Athlets = new HashSet<Athlet>();
            Competitions = new HashSet<Competition>();
        }

        public string Idcntr { get; set; } = null!;
        public string? Namecntr { get; set; }
        public byte[] SsmaTimeStamp { get; set; } = null!;

        public virtual ICollection<Athlet> Athlets { get; set; }
        public virtual ICollection<Competition> Competitions { get; set; }
    }
}
