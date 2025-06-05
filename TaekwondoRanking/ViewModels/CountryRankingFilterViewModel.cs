using TaekwondoRanking.ViewModels;
namespace TaekwondoRanking.ViewModels
{
    public class CountryRankingFilterViewModel
    {
        public string? SelectedCountry { get; set; }
        public string? SelectedAgeClass { get; set; }
        public string? SelectedGender { get; set; }
        public string? SelectedCategory { get; set; }

        public List<string> Countries { get; set; } = new();
        public List<string> AgeClasses { get; set; } = new();
        public List<string> Genders { get; set; } = new();
        public List<string> Categories { get; set; } = new();

        public List<AthletePointsViewModel> Results { get; set; } = new();
    }
}