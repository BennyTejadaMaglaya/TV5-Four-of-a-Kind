using Microsoft.AspNetCore.Mvc;

namespace TV5_VolunteerEventMgmtApp.Controllers
{
    public class ModalController : Controller
    {
        public IActionResult Index()
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
    }
}
