using System;
using System.Collections.Generic;

namespace TaekwondoRanking.Models
{
    public partial class AgeClass
    {
        public AgeClass()
        {
            Categories = new HashSet<Category>();
            SubCompetition1s = new HashSet<SubCompetition1>();
        }

        public string IdAgeClass { get; set; } = null!;
        public string? NameAgeClass { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<SubCompetition1> SubCompetition1s { get; set; }
    }
}
