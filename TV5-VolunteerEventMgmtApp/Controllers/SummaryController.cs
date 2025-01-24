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
            summary = FilterAttendanceSheet(summary, searchCity, searchDirector);
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



            var summaryValue = await summary.Select(x => new AttendanceSheetVM
            { // todo when things are fixed we'll set this to only the Total singers and not check the count
                TotalAttendees = x.TotalSingers != 0 ? x.TotalSingers : x.Location.SingerLocations.Where(l => l.Singer.isActive).Count(),
                Sheet = x,
                Percentage = new PercentageColorVM((double)x.Attendees.Count() / x.Location.SingerLocations.Where(l => l.Singer.isActive).Count()),
                
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

            return View(new AttendanceSummaryVM { LocationReport = SummarizeByLocation() , AttendanceSummaries=summaryValue});

        }

        [HttpGet]
        public  IActionResult ThisWeeksExcelReport(string reportTitle="This Weeks Attendance Report")
        {
            var summary = ThisWeeksAttendanceExcel();
            using (ExcelPackage excel = new ExcelPackage())
            {
                int numRows = summary.Count();

                var workSheet = excel.Workbook.Worksheets.Add("AttendanceReport");
                workSheet.Cells[3, 1].LoadFromCollection(summary, true);
                //workSheet.Cells[4, 1, numRows + 3, 2].Style.Font.Bold = true;
                workSheet.Row(3).Style.Font.Bold = true;
                //Add a title and timestamp at the top of the report
                workSheet.Cells[1, 1].Value = reportTitle;
                using (ExcelRange Rng = workSheet.Cells[1, 1, 1, 6])
                {
                    Rng.Merge = true; //Merge columns start and end range
                    Rng.Style.Font.Bold = true; //Font should be bold
                    Rng.Style.Font.Size = 18;
                    Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                }
                workSheet.Cells.AutoFitColumns();

                DateTime utcDate = DateTime.UtcNow; // timestamp code
                TimeZoneInfo esTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                DateTime localDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, esTimeZone);
                using (ExcelRange Rng = workSheet.Cells[2, 3])
                {
                    Rng.Value = "Created: " + localDate.ToShortTimeString() + " on " +
                        localDate.ToShortDateString();
                    Rng.Style.Font.Bold = true; 
                    Rng.Style.Font.Size = 18;
                    Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                }

                try
                {
                    // todo name the file bsed on the sort parameters when I implement that!
                    Byte[] theData = excel.GetAsByteArray();
                    string filename = $"Attendance-Summary-{DateUtilities.GetWeekStart().ToShortDateString()}.xlsx";
                    string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    return File(theData, mimeType, filename);
                }
                catch (Exception)
                {
                    return BadRequest("Could not build and download the file.");
                }
            }
            
        }

        private async Task<ICollection<AttendanceSheetVM>> ThisWeeksAttendance()
        {
            var summary = _context.AttendeesSheets
                .Where(s => s.StartTime >= DateUtilities.GetWeekStart() && s.StartTime <= DateUtilities.GetWeekEnd())
                .Include(s => s.Director)

                .Include(s => s.Location)
                .ThenInclude(s => s.SingerLocations)
                .ThenInclude(s => s.Singer)
                .AsNoTracking();
            
            return await summary.Select(x => new AttendanceSheetVM
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
                Available_Singers=x.TotalSingers != 0 ? x.TotalSingers : x.Location.SingerLocations.Where(l => l.Singer.isActive).Count(), 
                Director_Name=x.Director.NameSummary(), 
                Location_Name=x.Location.City,
                Total_Attendees =x.Attendees.Count(),
                Percentage_Attended= x.TotalSingers != 0 ? ((x.Attendees.Count / x.TotalSingers)).ToString("#0.##%") 
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

        private static IQueryable<AttendanceSheet> FilterAttendanceSheet(IQueryable<AttendanceSheet> sheets, string? searchCity, string? searchDirector)
        {
            if (!string.IsNullOrEmpty(searchDirector))
            {
                sheets = sheets.Where(s => s.Director.FirstName.ToLower().Contains(searchDirector.ToLower()) || s.Director.LastName.ToLower().Contains(searchDirector.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchCity))
            {
                sheets = sheets.Where(s => s.Location.City.ToLower().Contains(searchCity.ToLower()));
            }
            return sheets;
        }


        private LocationReportVM SummarizeByLocation()
        {
            // groupby query makes this code way more aids to read 
            var summary = _context.Locations
                .Include(L => L.AttendanceSheets)
                .ThenInclude(l => l.Attendees)
                
                .Include(s => s.SingerLocations)
                .ThenInclude(l => l.Singer)
                .Include(L => L.DirectorLocations)
                .ThenInclude(l => l.Director)
                .AsNoTracking();

            var summaryVM = new LocationReportVM (new List<LocationReportItem>() );

            foreach(var l in summary)
            {
               
                var vm = new LocationReportItem { 
                    Average_Attendees =  l.AttendanceSheets.Average(s => s.Attendees.Count()) ,
                    City = l.City,
                    Current_Total_Singers = l.SingerLocations.Where(s => s.Singer.isActive).Count(),

                };
                Console.WriteLine(vm.Average_Attendees);
                summaryVM.Items.Add(vm);
            }

            var low = summaryVM.Items.OrderBy(s => s.Average_Attendees).FirstOrDefault();
            var high = summaryVM.Items.OrderByDescending(s => s.Average_Attendees).FirstOrDefault();
            summaryVM.MinAvgAttendance = new AverageValue { Average = low.Average_Attendees, Name = low.City };

            summaryVM.MaxAvgAttendance = new AverageValue { Average = high.Average_Attendees, Name = high.City };


            return summaryVM;
        }



    }
}
