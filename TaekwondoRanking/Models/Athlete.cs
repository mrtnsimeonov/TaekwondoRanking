using System;
using System.Collections.Generic;

namespace TaekwondoRanking.Models
{
    public partial class Athlete
    {
        public Athlete()
        {
            Results = new HashSet<Result>();
        }

        public string IdAthlete { get; set; } = null!;
        public string? Country { get; set; }
        public string? Name { get; set; }

        public virtual Country? CountryNavigation { get; set; }
        public virtual ICollection<Result> Results { get; set; }
    }
}
