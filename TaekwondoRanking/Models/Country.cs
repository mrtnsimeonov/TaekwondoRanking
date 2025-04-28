using System;
using System.Collections.Generic;

namespace TaekwondoRanking.Models
{
    public partial class Country
    {
        public Country()
        {
            Athletes = new HashSet<Athlete>();
            Competitions = new HashSet<Competition>();
        }

        public string IdCountry { get; set; } = null!;
        public string? NameCountry { get; set; }

        public virtual ICollection<Athlete> Athletes { get; set; }
        public virtual ICollection<Competition> Competitions { get; set; }
    }
}
