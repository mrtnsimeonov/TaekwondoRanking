using System.Collections.Generic;
using TaekwondoRanking.ViewModels;

namespace TaekwondoRanking.ViewModels
{
    public class CountryRankingFilterViewModel
    {
        public string? SelectedCountry { get; set; }
        public List<string> Countries { get; set; } = new();
        public List<AthletePointsViewModel> Results { get; set; } = new();  // ✅ ADD THIS LINE
    }
}
