namespace TaekwondoRanking.ViewModels
{
    public class ContinentalRankingFilterViewModel
    {
        public string SelectedAgeClass { get; set; }
        public string SelectedGender { get; set; }
        public string SelectedCategory { get; set; }

        public List<string> AgeClasses { get; set; }
        public List<string> Genders { get; set; }
        public List<string> Categories { get; set; }

        public List<AthletePointsViewModel> Results { get; set; } = new();

        public string? SearchQuery { get; set; }

        public List<string> Continents { get; set; } = new List<string>
        {
            "Africa", "Asia", "Europe", "North America", "South America", "Oceania"
        };

        public string SelectedContinent { get; set; }

    }

}