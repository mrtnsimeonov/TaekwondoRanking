using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using TaekwondoRanking.Models;
using TaekwondoRanking.Views;

namespace TaekwondoRanking.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Bulgaria()
        {
            // Mock data for dropdown; replace with database query
            var categories = new List<SelectListItem>
        {
            new SelectListItem { Value = "Junior", Text = "Junior Division" },
            new SelectListItem { Value = "Cadet", Text = "Cadet Division" },
            new SelectListItem { Value = "CadetHeight", Text = "Cadet Height Division" }
        };

            var model = new FilterViewModel
            {
                Categories = categories
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Bulgaria(FilterViewModel model)
        {
            // Filter data based on model.SelectedCategory
            // Example: Query database with the selected category
            var filteredResults = $"You selected: {model.SelectedCategory}";

            ViewBag.Message = filteredResults;
            return View(model);
        }

        public IActionResult Europe() => View("~/Views/Regions/Europe.cshtml");

        public IActionResult World() => View("~/Views/Regions/World.cshtml");

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }


    }
}