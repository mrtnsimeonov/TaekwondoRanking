using System;
using System.Collections.Generic;

namespace TaekwondoRanking.Models
{
    public partial class Athlet
    {
        public Athlet()
        {
            Results = new HashSet<Result>();
        }

        public string Idathlet { get; set; } = null!;
        public string? Country { get; set; }
        public string? Name { get; set; }
        public byte[] SsmaTimeStamp { get; set; } = null!;

        public virtual Country? CountryNavigation { get; set; }
        public virtual ICollection<Result> Results { get; set; }
    }
}
