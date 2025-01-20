using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TV5_VolunteerEventMgmtApp.Data;
using TV5_VolunteerEventMgmtApp.Models;
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
        public async Task<IActionResult> Attendance(DateTime? startWeek, DateTime? endWeek)
        {
            // This summary grabs all sheets created this week for a quick overview 
            // get each loctions attendance
            // get total active singers (AttendanceSheet.SingerLocations.Count where singer is active)
           
            //sort by week steps
            // 1. take in a date and get the weekstart from that date
            // 2. filter by that week instead of the default this week

            // if theres no start week and no end week we default to this week otherwise we use the range

            
            var summary = _context.AttendeesSheets
                        .Include(i => i.Attendees)
                        .Include(a => a.Location)
                        .ThenInclude(l => l.SingerLocations)
                        .ThenInclude(l => l.Singer)
                        .Include(b => b.Venue)
                        .AsNoTracking();

            FilterDateRange(startWeek, endWeek, ref summary);

            if (startWeek.HasValue)
            {
                ViewData["startWeek"] = startWeek.Value;
            }
            if (endWeek.HasValue)
            {
                ViewData["endWeek"] = endWeek.Value;
            }

                        

            var summaryValue = await summary.Select(x => new AttendanceSheetSummaryVM
            {
                TotalAttendees = x.Location.SingerLocations.Where(l => l.Singer.isActive).Count(),
                Sheet = x,
                Percentage = new PercentageColor((double)x.Attendees.Count() / (double)x.Location.SingerLocations.Where(l => l.Singer.isActive).Count())
            })
                        .ToListAsync();

            ViewData["summaryTitle"] = (!startWeek.HasValue && !endWeek.HasValue) ? "This Weeks Attendance Summary" 
                : $"Attendance Summary {SummaryUtilities.DateRangeMessage(startWeek, endWeek)}";

            // future:
            // Allow searching/filtering for location on this page

            return View(summaryValue);

        }
        

        private static void FilterDateRange(DateTime? start, DateTime? end, ref IQueryable<AttendanceSheet> sheets)
        {
            // if theres no start week and no end week we default to this week otherwise we use the range
            if (!start.HasValue && !end.HasValue)
            {
                sheets = sheets.Where(s => s.StartTime >= DateUtilities.GetWeekStart() && s.StartTime <= DateUtilities.GetWeekEnd());
                return;
            }
            if (start.HasValue)
            {
                sheets = sheets.Where(s => s.StartTime >= start);
                Console.WriteLine(sheets.Count().ToString() + " Start");
            }
            if (end.HasValue)
            { 
                sheets = sheets.Where(sheets => sheets.StartTime <= end.Value);
                Console.WriteLine(sheets.Count().ToString() + " End");
            }
            
        }



    }
}
