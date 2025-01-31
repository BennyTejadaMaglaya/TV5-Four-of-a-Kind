using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TV5_VolunteerEventMgmtApp.Models;

namespace TV5_VolunteerEventMgmtApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public ActionResult LoadPartial(string partialViewName)
        {
            // Validate the partialViewName to prevent unauthorized access
            var allowedPartials = new[] { "_AttendanceSheetPartial", "_SingerPartial", "_CsvImportPartial", "_DirectorPartial", "_LocationPartial" };
            if (!allowedPartials.Contains(partialViewName))
            {
                return new BadRequestResult();
            }

            return PartialView($"~/Views/Shared/{partialViewName}.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
