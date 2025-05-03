using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaekwondoRanking.Models;
using TaekwondoRanking.ViewModels;

namespace TaekwondoRanking.Controllers
{
    public class RegionsController : Controller
    {
        private readonly CompetitionDbContext _context;

        public RegionsController(CompetitionDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> World()
        {
            var model = new WorldRankingFilterViewModel
            {
                AgeClasses = await _context.Categories.Select(c => c.AgeClass).Distinct().ToListAsync(),
                Genders = await _context.Categories.Select(c => c.Mf).Distinct().ToListAsync(),
                Categories = new List<string>(),
                SearchQuery = "", // Added for search input
                Results = new List<AthletePointsViewModel>()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> World(WorldRankingFilterViewModel model, string? reset, string? search)
        {
            // Reset logic
            if (!string.IsNullOrEmpty(reset))
            {
                model.SelectedAgeClass = null;
                model.SelectedGender = null;
                model.SelectedCategory = null;
                model.SearchQuery = null;
                model.Results = null;

                model.AgeClasses = await _context.Categories.Select(c => c.AgeClass).Distinct().ToListAsync();
                model.Genders = await _context.Categories.Select(c => c.Mf).Distinct().ToListAsync();
                model.Categories = new List<string>();
                return View(model);
            }

            // Search logic
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
                    .Where(g =>
                        g.Key.Name.ToLower().Contains(searchLower))
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

                return View(model);
            }

            // Filtered results logic
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

            model.AgeClasses = await _context.Categories.Select(c => c.AgeClass).Distinct().ToListAsync();
            model.Genders = await _context.Categories.Select(c => c.Mf).Distinct().ToListAsync();
            model.Categories = await _context.Categories
                .Where(c => c.AgeClass == model.SelectedAgeClass && c.Mf == model.SelectedGender)
                .Select(c => c.NameCategory)
                .Distinct()
                .ToListAsync();

            model.Results = athletes;

            return View(model);
        }

        public IActionResult Bulgaria()
        {
            return View();
        }

        public IActionResult Europe()
        {
            return View();
        }
    }
}
