namespace TaekwondoRanking.ViewModels
{
    public class TournamentHierarchyViewModel
    {
        public int Year { get; set; }
        public List<TournamentViewModel> Tournaments { get; set; } = new();
    }

    public class TournamentViewModel
    {
        public int TournamentId { get; set; }
        public string TournamentName { get; set; }
        public List<AgeClassViewModel> AgeClasses { get; set; } = new();
    }

    public class AgeClassViewModel
    {
        public string AgeClassName { get; set; }
        public List<CategoryViewModel> Categories { get; set; } = new();
    }

    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}