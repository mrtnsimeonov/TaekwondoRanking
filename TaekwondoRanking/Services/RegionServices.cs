using Microsoft.EntityFrameworkCore;
using TaekwondoRanking.Models;
using TaekwondoRanking.Services;
using TaekwondoRanking.ViewModels;
using TaekwondoRanking.Helpers;

public class RegionService : IRegionService
{
    private readonly CompetitionDbContext _context;

    public RegionService(CompetitionDbContext context)
    {
        _context = context;
    }

    // This method can stay as it was, it doesn't deal with the temporary data.
    public async Task<WorldRankingFilterViewModel> BuildInitialWorldRankingModelAsync()
    {
        return new WorldRankingFilterViewModel
        {
            AgeClasses = await _context.Categories.Select(c => c.AgeClass).Distinct().ToListAsync(),
            Genders = await _context.Categories.Select(c => c.Mf).Distinct().ToListAsync(),
            Categories = new List<string>(),
            SearchQuery = "",
            Results = new List<AthletePointsViewModel>()
        };
    }

    private IQueryable<Result> GetFilteredResultsQuery(string? selectedAgeClass, string? selectedGender, string? selectedCategory, string? searchQuery, string? selectedCountry)
    {
        IQueryable<Result> query = _context.Results
            .Include(r => r.IdAthleteNavigation)
                .ThenInclude(a => a.CountryNavigation)
            .Include(r => r.IdSubCompetition2Navigation)
                .ThenInclude(sc2 => sc2.IdCategoryNavigation)
            .AsQueryable();

        // ✅ FINAL FIX: Directly check if the string Keys of our dictionary contain the string IdAthlete.
        // EF Core will translate this to an efficient SQL "WHERE IN" or "WHERE NOT IN" clause.
        if (!TemporaryDeletionManager.TemporarilyDeletedAthleteIds.IsEmpty)
        {
            var keys = TemporaryDeletionManager.TemporarilyDeletedAthleteIds.Keys;
            query = query.Where(r => !keys.Contains(r.IdAthlete));
        }

        // Apply other database-side filters
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            string searchLower = searchQuery.ToLower();
            query = query.Where(r =>
                r.IdAthleteNavigation != null && r.IdAthleteNavigation.Name != null &&
                r.IdAthleteNavigation.Name.ToLower().Contains(searchLower)
            );
        }
        else
        {
            if (!string.IsNullOrEmpty(selectedAgeClass))
                query = query.Where(r => r.IdSubCompetition2Navigation.IdCategoryNavigation.AgeClass == selectedAgeClass);

            if (!string.IsNullOrEmpty(selectedGender))
                query = query.Where(r => r.IdSubCompetition2Navigation.IdCategoryNavigation.Mf == selectedGender);

            if (!string.IsNullOrEmpty(selectedCategory))
                query = query.Where(r => r.IdSubCompetition2Navigation.IdCategoryNavigation.NameCategory == selectedCategory);
        }

        if (!string.IsNullOrEmpty(selectedCountry))
        {
            query = query.Where(r => r.IdAthleteNavigation.CountryNavigation.NameCountry == selectedCountry);
        }

        return query;
    }

    public async Task<WorldRankingFilterViewModel> ApplyWorldRankingFiltersAsync(WorldRankingFilterViewModel model, string? reset, string? search)
    {
        if (!string.IsNullOrEmpty(reset))
        {
            return await BuildInitialWorldRankingModelAsync();
        }

        var resultsQuery = GetFilteredResultsQuery(model.SelectedAgeClass, model.SelectedGender, model.SelectedCategory, model.SearchQuery, null);

        model.Results = await resultsQuery
            .GroupBy(r => new
            {
                r.IdAthlete,
                r.IdAthleteNavigation.Name,
                Country = r.IdAthleteNavigation.CountryNavigation.NameCountry
            })
            .Select(g => new AthletePointsViewModel
            {
                IdAthlete = g.Key.IdAthlete,
                Name = g.Key.Name,
                Country = g.Key.Country,
                TotalPoints = g.Sum(x => (int?)x.Points) ?? 0
            })
            .OrderByDescending(a => a.TotalPoints)
            .ToListAsync();

        // Repopulate dropdowns
        model.AgeClasses = await _context.Categories.Select(c => c.AgeClass).Distinct().ToListAsync();
        model.Genders = await _context.Categories.Select(c => c.Mf).Distinct().ToListAsync();
        if (!string.IsNullOrEmpty(model.SelectedAgeClass) && !string.IsNullOrEmpty(model.SelectedGender))
        {
            model.Categories = await _context.Categories
                .Where(c => c.AgeClass == model.SelectedAgeClass && c.Mf == model.SelectedGender)
                .OrderBy(c => c.NameCategory)
                .Select(c => c.NameCategory).Distinct().ToListAsync();

        }
        else
        {
            model.Categories = new List<string>();
        }

        return model;
    }

    public async Task<CountryRankingFilterViewModel> ApplyCountryRankingFiltersAsync(CountryRankingFilterViewModel model, string? reset, string? search)
    {
        if (!string.IsNullOrEmpty(reset))
        {
            return await BuildInitialCountryRankingModelAsync();
        }

        var resultsQuery = GetFilteredResultsQuery(model.SelectedAgeClass, model.SelectedGender, model.SelectedCategory, null, model.SelectedCountry);

        model.Results = await resultsQuery
            .GroupBy(r => new
            {
                r.IdAthlete,
                r.IdAthleteNavigation.Name,
                Country = r.IdAthleteNavigation.CountryNavigation.NameCountry
            })
            .Select(g => new AthletePointsViewModel
            {
                IdAthlete = g.Key.IdAthlete,
                Name = g.Key.Name,
                Country = g.Key.Country,
                TotalPoints = g.Sum(x => (int?)x.Points) ?? 0
            })
            .OrderByDescending(a => a.TotalPoints)
            .ToListAsync();

        // Repopulate dropdowns
        model.Countries = await _context.Countries.OrderBy(c => c.NameCountry).Select(c => c.NameCountry).ToListAsync();
        model.AgeClasses = await _context.Categories.Select(c => c.AgeClass).Distinct().OrderBy(a => a).ToListAsync();
        model.Genders = await _context.Categories.Select(c => c.Mf).Distinct().OrderBy(g => g).ToListAsync();
        model.Categories = await _context.Categories
            .Where(c =>
                (string.IsNullOrEmpty(model.SelectedAgeClass) || c.AgeClass == model.SelectedAgeClass) &&
                (string.IsNullOrEmpty(model.SelectedGender) || c.Mf == model.SelectedGender))
            .OrderBy(c => c.NameCategory)
            .Select(c => c.NameCategory)
            .Distinct()
            .ToListAsync();


        return model;
    }

    // BuildInitialCountryRankingModelAsync remains largely the same but can also use the helper
    public async Task<CountryRankingFilterViewModel> BuildInitialCountryRankingModelAsync()
    {
        var resultsQuery = GetFilteredResultsQuery(null, null, null, null, null);

        var athletes = await resultsQuery
            .GroupBy(r => new
            {
                r.IdAthlete,
                r.IdAthleteNavigation.Name,
                Country = r.IdAthleteNavigation.CountryNavigation.NameCountry
            })
            .Select(g => new AthletePointsViewModel
            {
                IdAthlete = g.Key.IdAthlete,
                Name = g.Key.Name,
                Country = g.Key.Country,
                TotalPoints = (int)(g.Sum(r => r.Points) ?? 0)
            })
            .OrderByDescending(a => a.TotalPoints)
            .ToListAsync();

        return new CountryRankingFilterViewModel
        {
            Countries = await _context.Countries.OrderBy(c => c.NameCountry).Select(c => c.NameCountry).ToListAsync(),
            AgeClasses = await _context.Categories.Select(c => c.AgeClass).Distinct().ToListAsync(),
            Genders = await _context.Categories.Select(c => c.Mf).Distinct().ToListAsync(),
            Categories = await _context.Categories.Select(c => c.NameCategory).Distinct().ToListAsync(),
            Results = athletes
        };
    }
}