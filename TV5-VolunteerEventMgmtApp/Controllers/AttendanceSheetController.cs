using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TV5_VolunteerEventMgmtApp.Data;
using TV5_VolunteerEventMgmtApp.Models;
using TV5_VolunteerEventMgmtApp.Utilities;
using TV5_VolunteerEventMgmtApp.ViewModels;
using TV5_VolunteerEventMgmtApp.CustomControllers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using String = System.String;
using Humanizer;

namespace TV5_VolunteerEventMgmtApp.Controllers
{
	public class AttendanceSheetController : ElephantController

	{
		private readonly VolunteerEventMgmtAppDbContext _context;

		public AttendanceSheetController(VolunteerEventMgmtAppDbContext context)
		{
			_context = context;
		}

		private static readonly string[] DurationItems = ["30", "60", "90", "120", "150", "180"];

		private static readonly string[] DefaultColors = ["#FFCCD8", "#D4F6FF", "#D6FFD0", "#FFF0B0", "#FFD8AF", "#ECE0FF", "#ECE4CC", "#ECECEC"];

		// GET: AttendanceSheet
		public async Task<IActionResult> Index(int? LocationId, int? page, int? pageSizeID,
			string? actionButton, string? searchDirector, DateTime? startTime, DateTime? endTime, string sortDirection = "desc", string sortField = "Date", 
			string currentTab = "list"
			)
		{
			// Selected tab
			ViewData["currentTab"] = currentTab;

			// List of sort options
			string[] sortOptions = new[] { "Date", "Location", "Director" };

			// Filtering data
			ViewData["BtnBg"] = "btn-outline-dark";
			ViewData["BtnText"] = "Filters";
			int numberFilters = 0;

			ViewData["LocationID"] = new SelectList(_context.Locations.OrderBy(l => l.City), "ID", "City");

			var attendanceSheets = _context.AttendeesSheets
				.Include(a => a.Director)
				.Include(a => a.Location)
				.Include(a => a.Attendees)
				.OrderByDescending(a => a.StartTime)
				.AsNoTracking();

			// Filters
			if (LocationId.HasValue)
			{
				attendanceSheets = attendanceSheets.Where(a => a.LocationId == LocationId);
				numberFilters++;
			}
			if (numberFilters != 0)
			{
				ViewData["BtnBg"] = "btn-dark";
				ViewData["BtnText"] = $"{numberFilters} Filter{(numberFilters > 1 ? "s" : "")} Applied";
				ViewData["ShowFilter"] = " show";
			}
			if (!string.IsNullOrEmpty(searchDirector))
			{
				attendanceSheets = attendanceSheets.Where(s =>
				s.Director.FirstName.ToLower().Contains(searchDirector.ToLower())
				|| s.Director.LastName.ToLower().Contains(searchDirector.ToLower()));
				numberFilters++;
			}
			if (startTime.HasValue)
			{
				attendanceSheets = attendanceSheets.Where(s => s.StartTime >= startTime.Value);
				ViewData["startTime"] = startTime.Value.ToString("yyyy-MM-ddTHH:mm");
			}
			if (endTime.HasValue)
			{
				attendanceSheets = attendanceSheets.Where(sheets => sheets.StartTime <= endTime.Value);
				ViewData["endTime"] = endTime.Value.ToString("yyyy-MM-ddTHH:mm");
			}
			

			// Check if there is a call for a change of filtering or sorting
			if (!String.IsNullOrEmpty(actionButton)) // Form Submitted
			{
				page = 1; // Reset page to start if filtering or sorting

				if (sortOptions.Contains(actionButton)) // Change of sort is requested
				{
					if (actionButton == sortField) // Reverse order on same field
					{
						sortDirection = sortDirection == "asc" ? "desc" : "asc";
					}
					else
					{
						if (actionButton == "Date")
						{
							sortDirection = "desc";
						}
						else if (actionButton == "Location")
						{
							sortDirection = "asc";
						}
						if(actionButton == "Director")
						{
                            sortDirection = "asc";
                        }
					}
					sortField = actionButton; // Sort by the button clicked
				}
			}

			// Sort by
			if (sortField == "Location")
			{
				if (sortDirection == "desc")
				{
					attendanceSheets = attendanceSheets
						.OrderByDescending(a => a.Location.City)
						.ThenBy(a => a.StartTime);
				}
				else
				{
					attendanceSheets = attendanceSheets
						.OrderBy(a => a.Location.City)
						.ThenByDescending(a => a.StartTime);
				}
			}
			if (sortField == "Date")
			{
				if (sortDirection == "desc")
				{
					attendanceSheets = attendanceSheets
						.OrderByDescending(a => a.StartTime)
						.ThenBy(a => a.Location.City);
				}
				else
				{
					attendanceSheets = attendanceSheets
						.OrderBy(a => a.StartTime)
						.ThenByDescending(a => a.Location.City);
				}
			}
			if (sortField == "Director")
			{
				attendanceSheets = sortDirection == "desc" ? attendanceSheets
					.OrderByDescending(s => s.Director.FirstName).ThenByDescending(s => s.Director.LastName)
					:
					attendanceSheets
					.OrderBy(s => s.Director.FirstName).ThenBy(s => s.Director.LastName);


			}

			// Set sort for next time
			ViewData["sortField"] = sortField;
			ViewData["sortDirection"] = sortDirection;
			ViewData["searchDirector"] = searchDirector;
			// Colors
			ViewData["Colors"] = DefaultColors;

			// Handle Paging
			int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
			ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
			var pagedData = await PaginatedList<AttendanceSheet>.CreateAsync(attendanceSheets.AsNoTracking(), page ?? 1, pageSize);

			return View(pagedData);
		}

		// GET: AttendanceSheet/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var attendanceSheet = await _context.AttendeesSheets
				.Include(a => a.Director)
				.Include(a => a.Location)
				.Include(a => a.Attendees).ThenInclude(a => a.Singer)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (attendanceSheet == null)
			{
				return NotFound();
			}

			return View(attendanceSheet);
		}

		// GET: AttendanceSheet/Create
		public IActionResult Create()
		{
			DateTime now = DateTime.Now;
			DateTime thisHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
			
			ViewData["Duration"] = new SelectList(DurationItems, "60");
			AttendanceSheet attendanceSheet = new AttendanceSheet()
			{
				StartTime = thisHour
			};

			ViewData["LocationID"] = new SelectList(_context.Locations.Where(d => d.IsActive).OrderBy(l => l.City), "ID", "City");

			// Directors and singers lists will be empty at first,
			// Then they will be populated when the location is selected
			// ViewData["DirectorId"] = new SelectList(_context.Directors, "ID", "FullName");
			// PopulateSingerListBoxes(attendanceSheet);

			ViewData["SelectedSingers"] = new MultiSelectList(Enumerable.Empty<ListOptionVM>(), "ID", "DisplayText");
			ViewData["AvailableSingers"] = new MultiSelectList(Enumerable.Empty<ListOptionVM>(), "ID", "DisplayText");

			return View(attendanceSheet);

		}

		// POST: AttendanceSheet/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,DirectorId,Notes,StartTime,EndTime,LocationId," +
			"VenueId")] AttendanceSheet attendanceSheet, string[] selectedOptions, int Duration)
		{
			try
			{
				attendanceSheet.EndTime = attendanceSheet.StartTime.AddMinutes(Duration);
				UpdateAttendees(selectedOptions, attendanceSheet);

				if (ModelState.IsValid)
				{
					_context.Add(attendanceSheet);
					await _context.SaveChangesAsync();
					TempData["SuccessMessage"] = $"Attendance sheet created on {attendanceSheet.StartTime.Date}!";
					return RedirectToAction("Details", new { id = attendanceSheet.Id });
				}
				//else
				//{
				//	var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
				//	foreach (var error in errors)
				//	{
				//		ModelState.AddModelError("", error);
				//	}
				//}
			}
			catch
			{
				TempData["FailMessage"] = "Unable to create new attendance sheet.";
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
			}

			ViewData["DirectorId"] = new SelectList(_context.Directors, "ID", "FullName", attendanceSheet.DirectorId);
			ViewData["LocationId"] = new SelectList(_context.Locations, "ID", "City", attendanceSheet.LocationId); 
			ViewData["Duration"] = new SelectList(DurationItems, Duration);
			PopulateSingerListBoxes(attendanceSheet);
			return View(attendanceSheet);
		}

		// GET: AttendanceSheet/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var attendanceSheet = await _context.AttendeesSheets
				.Include(a => a.Attendees).ThenInclude(a => a.Singer)
				.FirstOrDefaultAsync(a => a.Id == id);

			if (attendanceSheet == null)
			{
				return NotFound();
			}
			// 
			var duration = attendanceSheet.EndTime - attendanceSheet.StartTime;
			var durationInMinutes = duration.TotalMinutes;
			ViewData["Duration"] = new SelectList(DurationItems, durationInMinutes.ToString());
			ViewData["LocationId"] = new SelectList(_context.Locations
				.OrderBy(l => l.City), "ID", "City", attendanceSheet.LocationId);
			var locationId = attendanceSheet.LocationId;
			// Director sfor that location
			ViewData["DirectorId"] = new SelectList(_context.Directors
				.Where(d => d.DirectorLocations
				.Any(ld => ld.LocationID == locationId)), "ID", "FullName", attendanceSheet.DirectorId);

			PopulateSingerListBoxes(attendanceSheet);
			return View(attendanceSheet);
		}

		// POST: AttendanceSheet/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, string[] selectedOptions, int Duration)
		{
			var attendanceSheetToUpdate = await _context.AttendeesSheets
				.Include(a => a.Attendees).ThenInclude(a => a.Singer)
				.FirstOrDefaultAsync(a => a.Id == id);

			if (id != attendanceSheetToUpdate.Id)
			{
				return NotFound();
			}

			attendanceSheetToUpdate.EndTime = attendanceSheetToUpdate.StartTime.AddMinutes(Duration);
			UpdateAttendees(selectedOptions, attendanceSheetToUpdate);

			if (await TryUpdateModelAsync<AttendanceSheet>(attendanceSheetToUpdate, "",
				a => a.DirectorId, a => a.Notes, a => a.StartTime,
				a => a.EndTime, a => a.LocationId))
			{
				try
				{
					await _context.SaveChangesAsync();
					TempData["SuccessMessage"] = $"Attendance sheet saved!";
					return RedirectToAction("Details", new { id = attendanceSheetToUpdate.Id });
				}
				catch (DbUpdateConcurrencyException)
				{
					TempData["FailMessage"] = "Unable to save.";
					if (!AttendanceSheetExists(attendanceSheetToUpdate.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
			}

			ViewData["DirectorId"] = new SelectList(_context.Directors, "ID", "FullName", attendanceSheetToUpdate.DirectorId);
			ViewData["LocationId"] = new SelectList(_context.Locations, "ID", "City", attendanceSheetToUpdate.LocationId);
			ViewData["Duration"] = new SelectList(DurationItems, Duration);
			PopulateSingerListBoxes(attendanceSheetToUpdate);
			return View(attendanceSheetToUpdate);
		}

		// GET: AttendanceSheet/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var attendanceSheet = await _context.AttendeesSheets
				.Include(a => a.Director)
				.Include(a => a.Location)
				.Include(a => a.Attendees).ThenInclude(a => a.Singer)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (attendanceSheet == null)
			{
				TempData["FailMessage"] = "Unable to delete.";
				return NotFound();
			}

			return View(attendanceSheet);
		}

		// POST: AttendanceSheet/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var attendanceSheet = await _context.AttendeesSheets.FindAsync(id);
			if (attendanceSheet != null)
			{
				_context.AttendeesSheets.Remove(attendanceSheet);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}


		// Populate Singer ListBoxes
		public void PopulateSingerListBoxes(AttendanceSheet attendanceSheet)
		{
			var locationId = attendanceSheet.LocationId;
			//var allSingers = _context.Singers;
			var allSingersInLocation = _context.Singers.Where(s => s.SingerLocation.Any(sl => sl.LocationId == locationId) && s.isActive == true);
			var currentSingersHS = new HashSet<int>(attendanceSheet.Attendees.Select(e => e.SingerId));

			var selected = new List<ListOptionVM>();
			var available = new List<ListOptionVM>();
			foreach (var c in allSingersInLocation)
			{
				if (currentSingersHS.Contains(c.Id))
				{
					selected.Add(new ListOptionVM
					{
						ID = c.Id,
						DisplayText = c.FirstName + " " + c.LastName
					});
				}
				else
				{
					available.Add(new ListOptionVM
					{
						ID = c.Id,
						DisplayText = c.FirstName + " " + c.LastName
					});
				}
			}
			ViewData["SelectedSingers"] = new MultiSelectList(selected.OrderBy(c => c.DisplayText), "ID", "DisplayText");
			ViewData["AvailableSingers"] = new MultiSelectList(available.OrderBy(c => c.DisplayText), "ID", "DisplayText");
		}

		// Update attendees for an attendance sheet
		private void UpdateAttendees(string[] selectedOptions, AttendanceSheet attendanceSheetToUpdate)
		{
			if (selectedOptions == null)
			{
				attendanceSheetToUpdate.Attendees = new List<Attendee>();
				return;
			}

			var selectedOptionsHS = new HashSet<string>(selectedOptions);
			var currentOptionsHS = new HashSet<int>(attendanceSheetToUpdate.Attendees.Select(a => a.SingerId));
			foreach (var s in _context.Singers)
			{
				if (selectedOptionsHS.Contains(s.Id.ToString())) // it is selected
				{
					if (!currentOptionsHS.Contains(s.Id)) // but not currently in the group class - Add it
					{
						attendanceSheetToUpdate.Attendees.Add(new Attendee
						{
							SingerId = s.Id,
							AttendenceSheetId = attendanceSheetToUpdate.Id
						});
					}
				}
				else // not selected
				{
					if (currentOptionsHS.Contains(s.Id)) // but is currently in the group class - Remove it
					{
						Attendee singerToRemove = attendanceSheetToUpdate.Attendees
							.FirstOrDefault(a => a.SingerId == s.Id);
						if (singerToRemove != null)
						{
							_context.Remove(singerToRemove);
						}
					}
				}
			}
		}

		public async Task<IActionResult> GetSingersByLocation(int locationId)
		{
			var singers = await _context.Singers
				.Where(s => s.SingerLocation.Any(sl => sl.LocationId == locationId) && s.isActive == true)
				.Select(s => new { s.Id, s.FirstName, s.LastName })
				.OrderBy(s => s.FirstName).ThenBy(s => s.LastName)
				.ToListAsync();

			return Json(singers);
		}

		public async Task<IActionResult> GetDirectorsByLocation(int locationId)
		{
			var singers = await _context.Directors
				.Where(d => d.DirectorLocations.Any(dl => dl.LocationID == locationId) && d.IsActive == true)
				.Select(d => new { d.ID, d.FirstName, d.LastName })
				.OrderBy(d => d.FirstName).ThenBy(d => d.LastName)
				.ToListAsync();

			return Json(singers);
		}

		public async Task<JsonResult> GetAttendanceByLocation(int locationId)
		{
			var attendanceHistory = await _context.AttendeesSheets
				.Include(a => a.Director)
				.Include(a => a.Location)

				.Include(a => a.Attendees)
				.Where(a => a.LocationId == locationId)
				.Select(a => new
				{
                    id = a.Id,
                    Title = (a.Location != null ? a.Location.City : "N/A") + $" Attendance ({a.Attendees.Count} / {_context.Singers.Count(s => s.SingerLocation.Any(sl => sl.LocationId == locationId))})",
                    Start = a.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),
					End = a.EndTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    ExtendedProps = new {
						AttendancePercentage = (a.Attendees.Count * 100) / _context.Singers.Count(s => s.SingerLocation.Any(sl => sl.LocationId == a.LocationId)),
                        LocationId = a.LocationId
                    }
                }).ToListAsync();

			return Json(attendanceHistory);
		}

		public JsonResult GetSingerCountByLocation(int locationId)
		{
			var singerCount = _context.Singers
				.Where(s => s.SingerLocation.Any(sl => sl.LocationId == locationId))
				.Count();

			return Json(singerCount);
		}

		public async Task<JsonResult> GetAllAttendance()
		{
			var attendanceHistory = await _context.AttendeesSheets
				.Include(a => a.Director)
				.Include(a => a.Location)

				.Include(a => a.Attendees)
				.Select(a => new
				{
                    id = a.Id,
                    Title = (a.Location != null ? a.Location.City : "N/A") + $" Attendance ({a.Attendees.Count} / {_context.Singers.Count(s => s.SingerLocation.Any(sl => sl.LocationId == a.LocationId))})",
                    Start = a.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),
					End = a.EndTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    ExtendedProps = new {
						AttendancePercentage = (a.Attendees.Count * 100) / _context.Singers.Count(s => s.SingerLocation.Any(sl => sl.LocationId == a.LocationId)),
                        LocationId = a.LocationId
                    }
                }).ToListAsync();

			return Json(attendanceHistory);
		}

		public JsonResult GetTotalSingerCount()
		{
			var totalSingerCount = _context.Singers.Count();
			return Json(totalSingerCount);
		}

        public JsonResult GetStatistics(int? locationId)
        {
            if (locationId.HasValue)
            {
                var totalSingers = _context.Singers.Count(s => s.SingerLocation.Any(sl => sl.LocationId == locationId.Value));
                var attendanceCounts = _context.AttendeesSheets
                    .Where(a => a.LocationId == locationId.Value)
                    .Select(a => a.Attendees.Count)
                    .ToList();

                var mean = attendanceCounts.Any() ? attendanceCounts.Average() : 0;
                var percentageAttendance = totalSingers > 0 ? (mean / totalSingers) * 100 : 0;

                return Json(new { mean, percentageAttendance });
            }
            else
            {
                var locationStats = _context.Locations.Select(location => new
                {
                    LocationName = location.City,
                    TotalSingers = _context.Singers.Count(s => s.SingerLocation.Any(sl => sl.LocationId == location.ID)),
                    AttendanceCounts = _context.AttendeesSheets
                        .Where(a => a.LocationId == location.ID)
                        .Select(a => a.Attendees.Count)
                        .ToList()
                })
                .ToList();

                var stats = locationStats.Select(ls => new
                {
                    ls.LocationName,
                    mean = ls.AttendanceCounts.Any() ? ls.AttendanceCounts.Average() : 0,
                    PercentageAttendance = ls.TotalSingers > 0 ? (ls.AttendanceCounts.Any() ? (ls.AttendanceCounts.Average() / ls.TotalSingers) * 100 : 0) : 0,
                    TotalSingers = ls.TotalSingers
                })
                .ToList();

                var highestMean = stats.OrderByDescending(s => s.PercentageAttendance * s.TotalSingers).FirstOrDefault();
                var lowestMean = stats.OrderBy(s => s.PercentageAttendance * s.TotalSingers).FirstOrDefault();

                return Json(new
                {
                    highestMeanLocation = highestMean?.LocationName,
                    highestmean = highestMean?.mean ?? 0,
                    highestPercentageAttendance = highestMean?.PercentageAttendance ?? 0,
                    lowestMeanLocation = lowestMean?.LocationName,
                    lowestmean = lowestMean?.mean ?? 0,
                    lowestPercentageAttendance = lowestMean?.PercentageAttendance ?? 0
                });
            }
        }

        public IActionResult Dashboard()
		{
			DateTime now = DateTime.Now;
			DateTime thisHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
			ViewData["StartTime"] = thisHour.ToString("MMMM d, yyyy");
			var viewModel = new DashboardVM
			{
				Locations = _context.Locations.ToList()
			};
			ViewData["Duration"] = new SelectList(DurationItems, "60");
			Singer singer = new Singer();
			PopulateAssignedLocations(singer);
			GetAllLocations();
			AttendanceSheet sheet = new AttendanceSheet()
			{
				StartTime = thisHour
			};
			PopulateSingerListBoxes(sheet);

			

			return View(viewModel);
		}

		private void PopulateAssignedLocations(Singer singer)
		{
			var allOptions = _context.Locations.Where(d => d.IsActive);
			var currentLocations = new HashSet<int>(singer.SingerLocation.Select(d => d.LocationId));
			var checkboxes = new List<CheckOptionVM>();
			foreach (var item in allOptions)
			{
				checkboxes.Add(new CheckOptionVM
				{
					ID = item.ID,
					DisplayText = item.City,
					Assigned = currentLocations.Contains(item.ID)
				});
			}
			ViewData["AvailableLocations"] = checkboxes;

		}
		private void GetAllLocations()
		{

			ViewData["AllLocations"] = GetLocationSelectList();
		}
		private SelectList GetLocationSelectList(bool activeOnly = true, int selected = -1)
		{
			return new SelectList(activeOnly ? _context.Locations.Where(s => s.IsActive) :
				_context.Locations, "ID", "City", selected);
		}

		public async Task<IActionResult> ShowData(int? year, int locationId = 1)
		{
			if(year == null)
			{
				year = DateTime.Now.Year;

            }
			
			var sheets = await _context.AttendeesSheets
				.Include(a => a.Attendees)
				.Where(a => a.LocationId == locationId && a.StartTime.Year == year)
				.AsNoTracking()
				.ToListAsync();

			var AttendanceByDate = sheets
				.GroupBy(a => new
				{
					Month = a.StartTime.Month,
					WeekOfMonth = ((a.StartTime.Day - 1) / 7 + 1)
				})
				.Select(a => new MonthWeekCellVM
				{
					Month = a.Key.Month,
					WeekOfMonth = a.Key.WeekOfMonth,
					AttendanceCount = a.Sum(sheet => sheet.Attendees.Count())
				})
				.ToList();

			int TotalAttendees = _context.SingerLocations.Where(a => a.LocationId == locationId).Count();

			// to avoid possible division by 0
			if (TotalAttendees == 0) TotalAttendees = 1;

			foreach(var week in AttendanceByDate)
			{
				float opacity = (float)week.AttendanceCount / TotalAttendees;
				OpacityUtility color = new OpacityUtility("#B500BE", opacity);
				week.ColorHex = color.HexWithOpacity;
			}

			AttendanceGraphVM vm = new AttendanceGraphVM
			{
				LocationId = locationId,
				Year = year,
				TotalRegistered = TotalAttendees,
				heatMapCells = AttendanceByDate
			};

			return PartialView(vm);
		}

		private bool AttendanceSheetExists(int id)
		{
			return _context.AttendeesSheets.Any(e => e.Id == id);
		}
	}
}
