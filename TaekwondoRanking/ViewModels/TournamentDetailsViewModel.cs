namespace TaekwondoRanking.ViewModels
{
    public class SubCompetition2ViewModel
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }


    public class SubCompetition1ViewModel
    {
        public string? AgeClassName { get; set; }
        public List<SubCompetition2ViewModel> Categories { get; set; } = new();
    }

    public class TournamentDetailsViewModel
    {
        public List<SubCompetition1ViewModel> SubCompetitions { get; set; } = new();
    }
}