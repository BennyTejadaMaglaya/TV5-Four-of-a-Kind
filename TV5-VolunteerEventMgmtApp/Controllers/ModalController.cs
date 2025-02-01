﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TV5_VolunteerEventMgmtApp.Data;

namespace TV5_VolunteerEventMgmtApp.Controllers
{
    public class ModalController : Controller
    {
        private readonly VolunteerEventMgmtAppDbContext _context;

        public ModalController(VolunteerEventMgmtAppDbContext context)
        {
            _context = context;
        }

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
            ViewBag.AvailableLocations = new SelectList(_context.Locations.Where(l => l.IsActive), "ID", "City");

            return PartialView($"~/Views/Shared/{partialViewName}.cshtml");
        }
    }
}
