using System;
using System.Collections.Generic;

namespace TaekwondoRanking.Models
{
    public partial class Result
    {
        public string IdAthlete { get; set; } = null!;
        public int IdSubCompetition2 { get; set; }
        public int? Place { get; set; }
        public double? Points { get; set; }

        public virtual Athlete IdAthleteNavigation { get; set; } = null!;
        public virtual SubCompetition2 IdSubCompetition2Navigation { get; set; } = null!;
    }
}
