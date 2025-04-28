using System;
using System.Collections.Generic;

namespace TaekwondoRanking.Models
{
    public partial class SubCompetition1
    {
        public SubCompetition1()
        {
            SubCompetition2s = new HashSet<SubCompetition2>();
        }

        public int IdSubCompetition1 { get; set; }
        public int? IdCompetition { get; set; }
        public DateTime? PlayDate { get; set; }
        public string AgeClass { get; set; } = null!;
        public string? Rank { get; set; }

        public virtual AgeClass AgeClassNavigation { get; set; } = null!;
        public virtual Competition? IdCompetitionNavigation { get; set; }
        public virtual ICollection<SubCompetition2> SubCompetition2s { get; set; }
    }
}
