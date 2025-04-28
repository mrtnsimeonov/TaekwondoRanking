using System;
using System.Collections.Generic;

namespace TaekwondoRanking.Models
{
    public partial class SubCompetition2
    {
        public SubCompetition2()
        {
            Results = new HashSet<Result>();
        }

        public int IdSubCompetition2 { get; set; }
        public int IdSubCompetition1 { get; set; }
        public string? IdCategory { get; set; }

        public virtual Category? IdCategoryNavigation { get; set; }
        public virtual SubCompetition1 IdSubCompetition1Navigation { get; set; } = null!;
        public virtual ICollection<Result> Results { get; set; }
    }
}
