using TaekwondoRanking.ViewModels;

public interface IRegionService
{
    Task<WorldRankingFilterViewModel> BuildInitialWorldRankingModelAsync();
    Task<WorldRankingFilterViewModel> ApplyWorldRankingFiltersAsync(WorldRankingFilterViewModel model, string? reset, string? search);
}
