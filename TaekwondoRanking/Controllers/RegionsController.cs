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
            return View();
        }
        [HttpGet]
        public IActionResult Country()
        {
            return View();
        }


    }
}