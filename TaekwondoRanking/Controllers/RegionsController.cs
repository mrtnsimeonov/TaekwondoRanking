using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaekwondoRanking.ViewModels;

namespace TaekwondoRanking.Controllers
{
    [Authorize]
    public class RegionsController : Controller
    {
        private readonly IRegionService _regionService;

        public RegionsController(IRegionService regionService)
        {
            _regionService = regionService;
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

            // Only trigger modal if search button was clicked
            if (submitType == "search")
            {
                TempData["TriggerModal"] = true;
            }

            return View(model);
        }



        [HttpGet]
        public IActionResult Country()
        {
            return View();
        }


    }
}