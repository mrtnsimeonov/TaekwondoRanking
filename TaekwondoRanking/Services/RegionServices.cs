using Microsoft.EntityFrameworkCore;
using TaekwondoRanking.Models;
using TaekwondoRanking.Services;
using TaekwondoRanking.ViewModels;

public class RegionService : IRegionService
{
    private readonly CompetitionDbContext _context;

    public RegionService(CompetitionDbContext context)
    {
        _context = context;
    }

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

    public async Task<WorldRankingFilterViewModel> ApplyWorldRankingFiltersAsync(WorldRankingFilterViewModel model, string? reset, string? search)
    {
        if (!string.IsNullOrEmpty(reset))
        {
            return await BuildInitialWorldRankingModelAsync();
        }

        if (!string.IsNullOrWhiteSpace(model.SearchQuery))
        {
            string searchLower = model.SearchQuery.ToLower();

            var searchResults = await _context.Results
                .Include(r => r.IdAthleteNavigation)
                    .ThenInclude(a => a.CountryNavigation)
                .GroupBy(r => new
                {
                    r.IdAthleteNavigation.IdAthlete,
                    r.IdAthleteNavigation.Name,
                    Country = r.IdAthleteNavigation.CountryNavigation.NameCountry
                })
                .Where(g => g.Key.Name != null && g.Key.Name.ToLower().Contains(searchLower))

                .Select(g => new AthletePointsViewModel
                {
                    IdAthlete = g.Key.IdAthlete,
                    Name = g.Key.Name,
                    Country = g.Key.Country,
                    TotalPoints = g.Sum(x => (int?)x.Points) ?? 0
                })
                .OrderByDescending(a => a.TotalPoints)
                .ToListAsync();

            model.Results = searchResults;
            model.AgeClasses = await _context.Categories.Select(c => c.AgeClass).Distinct().ToListAsync();
            model.Genders = await _context.Categories.Select(c => c.Mf).Distinct().ToListAsync();
            model.Categories = new List<string>();
            return model;
        }

        var categoriesQuery = _context.Categories.AsQueryable();

        if (!string.IsNullOrEmpty(model.SelectedAgeClass))
            categoriesQuery = categoriesQuery.Where(c => c.AgeClass == model.SelectedAgeClass);

        if (!string.IsNullOrEmpty(model.SelectedGender))
            categoriesQuery = categoriesQuery.Where(c => c.Mf == model.SelectedGender);

        if (!string.IsNullOrEmpty(model.SelectedCategory))
            categoriesQuery = categoriesQuery.Where(c => c.NameCategory == model.SelectedCategory);

        var filteredCategoryIds = await categoriesQuery.Select(c => c.IdCategory).ToListAsync();

        var athletes = await _context.Results
            .Include(r => r.IdAthleteNavigation)
                .ThenInclude(a => a.CountryNavigation)
            .Include(r => r.IdSubCompetition2Navigation)
                .ThenInclude(sc2 => sc2.IdCategoryNavigation)
            .Where(r =>
                r.IdSubCompetition2Navigation.IdCategoryNavigation.AgeClass == model.SelectedAgeClass &&
                r.IdSubCompetition2Navigation.IdCategoryNavigation.Mf == model.SelectedGender &&
                r.IdSubCompetition2Navigation.IdCategoryNavigation.NameCategory == model.SelectedCategory)
            .GroupBy(r => new
            {
                r.IdAthleteNavigation.IdAthlete,
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

        model.Results = athletes;
        model.AgeClasses = await _context.Categories.Select(c => c.AgeClass).Distinct().ToListAsync();
        model.Genders = await _context.Categories.Select(c => c.Mf).Distinct().ToListAsync();
        model.Categories = await _context.Categories
            .Where(c => c.AgeClass == model.SelectedAgeClass && c.Mf == model.SelectedGender)
            .Select(c => c.NameCategory)
            .Distinct()
            .ToListAsync();

        return model;
    }

    public async Task<CountryRankingFilterViewModel> BuildInitialCountryRankingModelAsync()
    {
        var countries = await _context.Countries
    .OrderBy(c => c.NameCountry)
    .Select(c => c.NameCountry)
    .ToListAsync();

        var athletes = _context.Results
            .GroupBy(r => r.IdAthlete)
            .Select(g => new AthletePointsViewModel
            {
                IdAthlete = g.Key.ToString(),
                Name = _context.Athletes.FirstOrDefault(a => a.IdAthlete == g.Key).Name,
                Country = _context.Athletes.FirstOrDefault(a => a.IdAthlete == g.Key).CountryNavigation.NameCountry,
                TotalPoints = (int)(g.Sum(r => r.Points) ?? 0)
            })
            .OrderByDescending(a => a.TotalPoints)
            .ToList();

        return new CountryRankingFilterViewModel
        {
            Countries = await _context.Countries.OrderBy(c => c.NameCountry).Select(c => c.NameCountry).ToListAsync(),
            AgeClasses = await _context.Categories.Select(c => c.AgeClass).Distinct().ToListAsync(),
            Genders = await _context.Categories.Select(c => c.Mf).Distinct().ToListAsync(),
            Categories = await _context.Categories.Select(c => c.NameCategory).Distinct().ToListAsync(),
            Results = athletes
        };

    }


    public async Task<CountryRankingFilterViewModel> ApplyCountryRankingFiltersAsync(CountryRankingFilterViewModel model, string? reset, string? search)
    {

        if (!string.IsNullOrEmpty(reset))
        {
            return await BuildInitialCountryRankingModelAsync();
        }




        // Get base list of results
        var results = _context.Results
            .Include(r => r.IdAthleteNavigation)
                .ThenInclude(a => a.CountryNavigation)
            .Include(r => r.IdSubCompetition2Navigation)
                .ThenInclude(sc2 => sc2.IdCategoryNavigation)
            .AsQueryable();

        // Apply Age Class + Gender + Category filters
        if (!string.IsNullOrEmpty(model.SelectedAgeClass))
        {
            results = results.Where(r => r.IdSubCompetition2Navigation.IdCategoryNavigation.AgeClass == model.SelectedAgeClass);
        }

        if (!string.IsNullOrEmpty(model.SelectedGender))
        {
            results = results.Where(r => r.IdSubCompetition2Navigation.IdCategoryNavigation.Mf == model.SelectedGender);
        }

        if (!string.IsNullOrEmpty(model.SelectedCategory))
        {
            results = results.Where(r => r.IdSubCompetition2Navigation.IdCategoryNavigation.NameCategory == model.SelectedCategory);
        }

        if (!string.IsNullOrEmpty(model.SelectedCountry))
        {
            results = results.Where(r => r.IdAthleteNavigation.CountryNavigation.NameCountry == model.SelectedCountry);
        }

        // Group and project final results
        var grouped = await results
            .GroupBy(r => new
            {
                r.IdAthleteNavigation.IdAthlete,
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

        // Populate dropdowns
        model.Countries = await _context.Countries
            .OrderBy(c => c.NameCountry)
            .Select(c => c.NameCountry)
            .ToListAsync();

        model.AgeClasses = await _context.Categories
            .Select(c => c.AgeClass)
            .Distinct()
            .OrderBy(a => a)
            .ToListAsync();

        model.Genders = await _context.Categories
            .Select(c => c.Mf)
            .Distinct()
            .OrderBy(g => g)
            .ToListAsync();

        // This is the cascading dropdown part
        model.Categories = await _context.Categories
            .Where(c =>
                (string.IsNullOrEmpty(model.SelectedAgeClass) || c.AgeClass == model.SelectedAgeClass) &&
                (string.IsNullOrEmpty(model.SelectedGender) || c.Mf == model.SelectedGender))
            .Select(c => c.NameCategory)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();

        model.Results = grouped;
        return model;
    }




}