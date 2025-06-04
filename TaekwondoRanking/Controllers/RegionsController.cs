using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaekwondoRanking.Services;
using TaekwondoRanking.ViewModels;
using TaekwondoRanking.Models;
using System.Linq;

namespace TaekwondoRanking.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IRegionService _regionService;
        private readonly CompetitionDbContext _context; // ADD: Inject DbContext

        public RegionsController(IRegionService regionService, CompetitionDbContext context)
        {
            _regionService = regionService;
            _context = context; // ADD
        }

        [HttpGet]
        public async Task<IActionResult> World()
        {
            var model = await _regionService.BuildInitialWorldRankingModelAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> World(WorldRankingFilterViewModel model, string? reset, string? search)
        {
            var updatedModel = await _regionService.ApplyWorldRankingFiltersAsync(model, reset, search);
            return View(updatedModel);
        }

        [HttpGet]
        public IActionResult Continental()
        {
            var model = new ContinentalRankingFilterViewModel
            {
                AgeClasses = new List<string> { "Junior", "Senior" },
                Genders = new List<string> { "Male", "Female" },
                Categories = new List<string> { "Flyweight", "Lightweight", "Heavyweight" }
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Continental(ContinentalRankingFilterViewModel model, string? submitType)
        {
            model.AgeClasses ??= new List<string> { "Junior", "Senior" };
            model.Genders ??= new List<string> { "Male", "Female" };
            model.Categories ??= new List<string> { "Flyweight", "Lightweight", "Heavyweight" };
            model.Continents ??= new List<string> { "Africa", "Asia", "Europe", "North America", "South America", "Oceania" };

            if (submitType == "search")
            {
                TempData["TriggerModal"] = true;
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Country()
        {
            var model = await _regionService.BuildInitialCountryRankingModelAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Country(CountryRankingFilterViewModel model, string? reset, string? search)
        {
            var updatedModel = await _regionService.ApplyCountryRankingFiltersAsync(model, reset, search);
            return View(updatedModel);
        }



    }
}
