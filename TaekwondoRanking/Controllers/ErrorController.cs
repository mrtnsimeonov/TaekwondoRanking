using Microsoft.AspNetCore.Mvc;

namespace TaekwondoRanking.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 401:
                    ViewBag.ErrorMessage = "Unauthorized access. You need to be logged in to access this page.";
                    return View("Error401");

                case 404:
                    ViewBag.ErrorMessage = "Page not found.";
                    return View("Error404");

                default:
                    ViewBag.ErrorMessage = "An unexpected error occurred.";
                    return View("Error");
            }
        }
    }

}
