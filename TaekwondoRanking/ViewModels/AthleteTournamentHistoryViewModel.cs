namespace TaekwondoRanking.ViewModels
{
    public class AthleteTournamentHistoryViewModel
    {
        public string CompetitionName { get; set; }
        public string RangeLabel { get; set; }
        public string Country { get; set; }
        public string AgeClass { get; set; }
        public string Category { get; set; }
        public int Place { get; set; }
        public double Points { get; set; }
        public DateTime FromDate { get; set; }
    }

}
