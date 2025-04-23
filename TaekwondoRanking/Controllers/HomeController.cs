﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaekwondoRanking.Models;

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
        public IActionResult Bulgaria() => View("~/Views/Regions/Bulgaria.cshtml");

        public IActionResult Europe() => View("~/Views/Regions/Europe.cshtml");

        public IActionResult World() => View("~/Views/Regions/World.cshtml");

    }
}