using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TV5_VolunteerEventMgmtApp.Data;
using TV5_VolunteerEventMgmtApp.Utilities;
using TV5_VolunteerEventMgmtApp.ViewModels;

namespace TV5_VolunteerEventMgmtApp.Controllers
{
    public class SummaryController : Controller
    {

        private readonly VolunteerEventMgmtAppDbContext _context;

        public SummaryController(VolunteerEventMgmtAppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Attendance()
        {
            // get each loctions attendance
            // get total active singers (AttendanceSheet.SingerLocations.Count where singer is active)
           

            // if we're 
            var start = DateUtilities.GetWeekStart();
            var end = DateUtilities.GetWeekEnd();
            var summary = await _context.AttendeesSheets
                        .Include(i => i.Attendees)
                        .Include(a => a.Location)
                        .ThenInclude(l => l.SingerLocations)
                        .ThenInclude(l => l.Singer)
                        .Include(b => b.Venue)
                        .Where(x => x.StartTime >= start && x.EndTime <= end)
                        .Select(x => new AttendanceSheetSummaryVM { TotalAttendees=x.Location.SingerLocations.Where(l => l.Singer.isActive).Count(), Sheet=x })
                        .ToListAsync();
            
            


            // future:
            // Allow searching/filtering for location on this page

            return View(summary);
        }
        
       

       
    }
}
