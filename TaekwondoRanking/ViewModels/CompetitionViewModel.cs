namespace TaekwondoRanking.ViewModels
{
    public class CompetitionViewModel
    {
        public int IdCompetition { get; set; }
        public string? NameCompetition { get; set; }
        public string? Country { get; set; }
        public string? RangeLabel { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? TillDate { get; set; }
    }
}