using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Globalization;
using System.Linq;
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
            { // todo when things are fixed we'll set this to only the Total singers and not check the count
                TotalAttendees = x.TotalSingers != 0 ? x.TotalSingers : x.Location.SingerLocations.Where(l => l.Singer.isActive).Count(),
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
                ViewData["startWeek"] = startWeek.Value.ToString("yyyy-MM-ddTHH:mm");
            }
            if (endWeek.HasValue)
            {
                ViewData["endWeek"] = endWeek.Value.ToString("yyyy-MM-ddTHH:mm");
            }

            return View(summaryValue);

        }

        [HttpGet]
        public  IActionResult ThisWeeksExcelReport(string reportTitle="This Weeks Attendance Report")
        {
            var summary = ThisWeeksAttendanceExcel();
            using (ExcelPackage excel = new ExcelPackage())
            {
                

                var workSheet = excel.Workbook.Worksheets.Add("AttendanceReport");
                workSheet.Cells[3, 1].LoadFromCollection(summary, true);

                //Add a title and timestamp at the top of the report
                workSheet.Cells[1, 1].Value = reportTitle;
                using (ExcelRange Rng = workSheet.Cells[1, 1, 1, 6])
                {
                    Rng.Merge = true; //Merge columns start and end range
                    Rng.Style.Font.Bold = true; //Font should be bold
                    Rng.Style.Font.Size = 18;
                    Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                try
                {
                    Byte[] theData = excel.GetAsByteArray();
                    string filename = "Attendance-Summary-2025-01-22.xlsx";
                    string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    return File(theData, mimeType, filename);
                }
                catch (Exception)
                {
                    return BadRequest("Could not build and download the file.");
                }
            }
            
        }

        private async Task<ICollection<AttendanceSheetSummaryVM>> ThisWeeksAttendance()
        {
            var summary = _context.AttendeesSheets
                .Where(s => s.StartTime >= DateUtilities.GetWeekStart() && s.StartTime <= DateUtilities.GetWeekEnd())
                .Include(s => s.Director)

                .Include(s => s.Location)
                .ThenInclude(s => s.SingerLocations)
                .ThenInclude(s => s.Singer)
                .AsNoTracking();
            
            return await summary.Select(x => new AttendanceSheetSummaryVM
            { // todo when things are fixed we'll set this to only the Total singers and not check the count
                TotalAttendees = x.Attendees.Count(),
                Sheet = x,
                Percentage = new PercentageColorVM((double)x.Attendees.Count() / x.TotalSingers != 0 ? x.TotalSingers : x.Location.SingerLocations.Where(l => l.Singer.isActive).Count())
            }).ToListAsync();

        }

        private  ICollection<AttendanceSheetExcel> ThisWeeksAttendanceExcel()
        { // todo for speeds sake i will clean this up later but im just tryna get this excel shit working
            var summary = _context.AttendeesSheets
                .Where(s => s.StartTime >= DateUtilities.GetWeekStart() && s.StartTime <= DateUtilities.GetWeekEnd())
                .Include(s => s.Director)

                .Include(s => s.Location)
                .ThenInclude(s => s.SingerLocations)
                .ThenInclude(s => s.Singer)
                .AsNoTracking();

            return [.. summary.Select(x => new AttendanceSheetExcel { 
                AvailableSingers=x.TotalSingers != 0 ? x.TotalSingers : x.Location.SingerLocations.Where(l => l.Singer.isActive).Count(), 
                DirectorName=x.Director.NameSummary(), 
                LocationName=x.Location.City,
                TotalAttendees =x.Attendees.Count(),
                PercentageAttended= x.TotalSingers != 0 ? ((x.Attendees.Count / x.TotalSingers)).ToString("#0.##%") 
                :(((double)x.Attendees.Count() / x.Location.SingerLocations.Where(l => l.Singer.isActive).Count())).ToString("#0.##%")
            })];
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
