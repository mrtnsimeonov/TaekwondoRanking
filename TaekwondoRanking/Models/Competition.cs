using System;
using System.Collections.Generic;

namespace TaekwondoRanking.Models
{
    public partial class Competition
    {
        public Competition()
        {
            SubCompetition1s = new HashSet<SubCompetition1>();
        }

        public int IdCompetition { get; set; }
        public string? Country { get; set; }
        public string? NameCompetition { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? TillDate { get; set; }
        public string? RangeLabel { get; set; }

        public virtual Country? CountryNavigation { get; set; }
        public virtual ICollection<SubCompetition1> SubCompetition1s { get; set; }
    }
}
