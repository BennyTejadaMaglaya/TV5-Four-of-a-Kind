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
        public async Task<IActionResult> Attendance(
                                    DateTime? startWeek, 
                                    DateTime? endWeek,     
                                    string? actionButton,
                                    string? searchCity,
                                    string? searchDirector,
                                    string sortDirection = "asc",
                                    string sortField = "City")
        {
            /* TODOS
             * Sort by total attendees asc/desc
             * sort by total available singers asc/desc
             */
            
            var summary = _context.AttendeesSheets
                        .Include(i => i.Attendees)
                        .Include(a => a.Location)
                        .ThenInclude(l => l.SingerLocations)
                        .ThenInclude(l => l.Singer)
                        .Include(l => l.Director)
                        .AsNoTracking();

            FilterDateRange(startWeek, endWeek, ref summary);
            SortUtilities.SwapSortDirection(ref sortField, ref sortDirection, ["City", "Director"], actionButton);
            SortAttendanceSheets(ref summary, sortField, sortDirection);
            
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
                Percentage = new PercentageColorVM((double)x.Attendees.Count() / x.Location.SingerLocations.Where(l => l.Singer.isActive).Count())
            })
            .ToListAsync();


            ViewData["summaryTitle"] = (!startWeek.HasValue && !endWeek.HasValue) ? "This Weeks Attendance Summary" 
                : $"Attendance Summary {SummaryUtilities.DateRangeMessage(startWeek, endWeek)}";
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;
            ViewData["searchCity"] = searchCity;
            ViewData["searchDirector"] = searchDirector;
            if (startWeek.HasValue) // todo fix this ( think i
            {
                ViewData["startWeek"] = startWeek.Value.ToString("yyyy-MM-dd");
            }
            if (endWeek.HasValue)
            {
                ViewData["endWeek"] = endWeek.Value.ToString("yyyy-MM-dd");
            }

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
                //Console.WriteLine(sheets.Count().ToString() + " Start");
            }
            if (end.HasValue)
            { 
                sheets = sheets.Where(sheets => sheets.StartTime <= end.Value);
                //Console.WriteLine(sheets.Count().ToString() + " End");
            }
            
        }

        private static void SortAttendanceSheets(ref IQueryable<AttendanceSheet> sheets, string sortField, string sortDirection)
        {
            if (sortField == "Director")
            {
                sheets = sortDirection == "asc" ?
                    sheets.OrderByDescending(s => s.Director.FirstName).ThenByDescending(s => s.Director.LastName)
                    :
                    sheets.OrderBy(s => s.Director.FirstName).ThenBy(s => s.Director.LastName);
            }

            if (sortField == "City")
            {
                sheets = sortDirection == "asc" ?
                    sheets.OrderByDescending(s => s.Location.City) : sheets.OrderBy(s => s.Location.City);
            }
        }



    }
}
