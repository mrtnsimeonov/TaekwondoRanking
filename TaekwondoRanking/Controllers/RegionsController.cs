using Microsoft.AspNetCore.Mvc;

namespace TaekwondoRanking.Controllers
{
    public class RegionsController : Controller
    {
        public IActionResult Bulgaria()
        {
            return View();
        }

        public IActionResult Europe()
        {
            return View();
        }

        public IActionResult World()
        {
            return View();
        }
    }
}
