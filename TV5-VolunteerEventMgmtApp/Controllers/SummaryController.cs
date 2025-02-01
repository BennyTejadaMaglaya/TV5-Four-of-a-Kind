﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
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
            SortUtilities.SwapSortDirection(ref sortField, ref sortDirection, ["City", "Director", "Total Attended", "Total Signups"], actionButton);
            SortAttendanceSheets(ref summary, sortField, sortDirection);


            
            var summaryValue = await summary.Select(x => new AttendanceSheetVM
            { // todo when things are fixed we'll set this to only the Total singers and not check the count
                TotalAttendees = x.TotalSingers != 0 ? x.TotalSingers : x.Location.SingerLocations.Where(l => l.Singer.isActive).Count(),
                Sheet = x,
                Percentage = new PercentageColorVM((double)x.Attendees.Count() / x.Location.SingerLocations.Where(l => l.Singer.isActive).Count()),

            }).ToListAsync();
            SortSummarizedAttendanceSheet(ref summaryValue, sortField, sortDirection);

            var filters = AnyFilters(startWeek, endWeek, actionButton, searchCity, searchDirector, sortDirection, sortField);
            ViewData["summaryTitle"] = filters
                ? "Attendance Summary" : "This Weeks Attendance Summary";
            if (filters)
            {
                ViewData["ShowFilter"] = " show";
            }
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;
            LocationSelectList(searchCity);
            ViewData["searchCity"] = searchCity;
            ViewData["searchDirector"] = searchDirector;
            ViewData["exportLink"] = Request.QueryString.ToString();
            if (startWeek.HasValue) 
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

        [HttpGet]
        public IActionResult ExportToExcel(DateTime? startWeek,
                                    DateTime? endWeek,
                                    string? actionButton,
                                    string? searchDirector,
                                    string sortDirection = "asc",
                                    string sortField = "City",
                                    string? searchCity = "All")
        {
            var sheets = _context.AttendeesSheets
                .Include(s => s.Director)
                .Include(s => s.Location)
                .ThenInclude(s => s.SingerLocations)
                .ThenInclude(s => s.Singer)
                .Include(s => s.Attendees)
                .AsNoTracking();


            FilterDateRange(startWeek, endWeek, ref sheets);
            var filteredSheets = FilterAttendanceSheet(sheets, searchCity, searchDirector);
            SortAttendanceSheets(ref filteredSheets, sortDirection, sortField);

            var lists = (sortDirection == "asc" ?
                filteredSheets.OrderByDescending(s => (double)s.Attendees.Count() / s.Location.SingerLocations.Where(l => l.Singer.isActive).Count())
                :
                filteredSheets.OrderBy(s => (double)s.Attendees.Count() / s.Location.SingerLocations.Where(l => l.Singer.isActive).Count())).ToList();


            var finalized = new List<AttendanceSheetExcel>();
            var l = new List<string>();
            Console.WriteLine(finalized.Count());
            foreach(var x in lists)
            {
                Console.WriteLine(x);
                finalized.Add(new AttendanceSheetExcel
                {
                    Available_Singers = x.TotalSingers != 0 ? x.TotalSingers : x.Location.SingerLocations.Where(l => l.Singer.isActive).Count(),
                    Director_Name = x.Director == null ? "No director (temp)" : x.Director.NameSummary(),
                    Location_Name = x.Location.City,
                    Total_Attendees = x.Attendees.Count(),
                    Percentage_Attended = /*x.TotalSingers != 0 ? ((x.Attendees.Count / x.TotalSingers)).ToString("#0.##%"):*/
               (((double)x.Attendees.Count() / x.Location.SingerLocations.Where(l => l.Singer.isActive).Count())).ToString("#0.##%")
                });
            }

           

            try
            {
                var excel = CreateExcel($"Attendance Summary {SummaryUtilities.DateRangeMessage(startWeek, endWeek)}", finalized);
                return File(excel.Data, excel.MimeType, excel.FileName);
            }
            catch
            {

            }

            return BadRequest(StaticMessages.UnableToCreateReport);
        }

        


        private FileData CreateExcel(string reportTitle, ICollection<AttendanceSheetExcel> summary)
        {
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

                // todo name the file bsed on the sort parameters when I implement that!
                Byte[] theData = excel.GetAsByteArray();
                string filename = $"Attendance-Summary-{DateUtilities.GetWeekStart().ToShortDateString()}.xlsx";
                string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return new FileData { MimeType=mimeType, Data=theData, FileName=filename };
                
            }
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

        private static void SortSummarizedAttendanceSheet(ref List<AttendanceSheetVM> sheets, string sortField, string sortDirection)
        {
            
            if(sortField == "Total Attended")
            {

                sheets = (sortDirection == "asc" ?
                    sheets.OrderByDescending(s => s.Percentage.Value) : sheets.OrderBy(s => s.Percentage.Value)).ToList();
            }
            if(sortField == "Total Signups")
            {
                sheets = (sortDirection == "asc" ?
                    sheets.OrderByDescending(s => s.TotalAttendees) : sheets.OrderBy(s => s.TotalAttendees)).ToList();
            }
            
        }

        private static IQueryable<AttendanceSheet> FilterAttendanceSheet(IQueryable<AttendanceSheet> sheets, string? searchCity, string? searchDirector)
        {
            if (!string.IsNullOrEmpty(searchDirector))
            {
                sheets = sheets.Where(s => s.Director.FirstName.ToLower().Contains(searchDirector.ToLower()) || s.Director.LastName.ToLower().Contains(searchDirector.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchCity) && searchCity != "All")
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

            var summaryVM = new LocationReportVM ();

            foreach (var l in summary)
            {
                if (l.AttendanceSheets.Count() == 0)
                {
                    summaryVM.Items.Add(new LocationReportItem { Average_Attendees = 0, City=l.City, Current_Active_Singers=0, Current_Inactive_Singers=0 });
                    continue;
                }

                var vm = new LocationReportItem
                {
                    Average_Attendees = l.AttendanceSheets.Average(s => s.Attendees.Count()),
                    City = l.City,
                    Current_Active_Singers = l.SingerLocations.Where(s => s.Singer.isActive).Count(),
                    Current_Inactive_Singers = l.SingerLocations.Where(s => !s.Singer.isActive).Count()
                };
                summaryVM.Items.Add(vm);
            }

            var low = summaryVM.Items.OrderBy(s => s.Average_Attendees).FirstOrDefault();
            var high = summaryVM.Items.OrderByDescending(s => s.Average_Attendees).FirstOrDefault();
            var lowSingerTotal = summaryVM.Items.OrderByDescending(s => s.Current_Active_Singers).FirstOrDefault();
            var highSingerTotal = summaryVM.Items.OrderBy(s => s.Current_Active_Singers).FirstOrDefault();
            summaryVM.MinAvgAttendance = new NamedValue { Value = low.Average_Attendees, Name = low.City };
            summaryVM.ActiveSingers = summaryVM.Items.Sum(s => s.Current_Active_Singers + s.Current_Inactive_Singers); 
            // Not total in DB just counting total singerLocation  relationships

            summaryVM.MinTotalSingers = new NamedValue { Value = lowSingerTotal.Current_Active_Singers, Name=lowSingerTotal.City };
            summaryVM.MaxTotalSingers = new NamedValue { Value = highSingerTotal.Current_Active_Singers, Name = highSingerTotal.City };
            summaryVM.MaxAvgAttendance = new NamedValue { Value = high.Average_Attendees, Name = high.City };


            return summaryVM;
        }


        private void LocationSelectList(string current)
        {
            
            var locations = _context.Locations.ToList();
            if (!string.IsNullOrEmpty(current) && current != "All")
            {
                Console.WriteLine("pssing");
                for (int i = 0; i < locations.Count; i++)
                {
                    if(locations[i].City == current)
                    {
                        Console.WriteLine("pssiffng");
                        ViewBag.AvailableLocations = new SelectList(locations, "City", "City", locations[i].City);
                        return;
                    }
                }
            }


            ViewBag.AvailableLocations = new SelectList(locations, "City", "City");
        }

        private bool AnyFilters(DateTime? startWeek,
                                    DateTime? endWeek,
                                    string? actionButton,
                                    string? searchCity,
                                    string? searchDirector,
                                    string sortDirection = "asc",
                                    string sortField = "City")

        {
            if (startWeek.HasValue || endWeek.HasValue) 
            {
                return true;
            }

            if (!string.IsNullOrEmpty(searchCity) || !string.IsNullOrEmpty(searchDirector))
            {
                return true;
            }


            return false;
        }


    }
}
