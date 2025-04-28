using System;
using System.Collections.Generic;

namespace TaekwondoRanking.Models
{
    public partial class Category
    {
        public Category()
        {
            SubCompetition2s = new HashSet<SubCompetition2>();
        }

        public string IdCategory { get; set; } = null!;
        public string? AgeClass { get; set; }
        public string? Mf { get; set; }
        public string? NameCategory { get; set; }

        public virtual AgeClass? AgeClassNavigation { get; set; }
        public virtual ICollection<SubCompetition2> SubCompetition2s { get; set; }
    }
}
