using System.Threading.Tasks;
using TaekwondoRanking.ViewModels;

namespace TaekwondoRanking.Services
{
    public interface IRegionService
    {
        Task<WorldRankingFilterViewModel> BuildInitialWorldRankingModelAsync();
        Task<WorldRankingFilterViewModel> ApplyWorldRankingFiltersAsync(WorldRankingFilterViewModel model, string? reset, string? search);

        Task<CountryRankingFilterViewModel> BuildInitialCountryRankingModelAsync();
        Task<CountryRankingFilterViewModel> ApplyCountryRankingFiltersAsync(CountryRankingFilterViewModel model, string? reset, string? search);


    }
}