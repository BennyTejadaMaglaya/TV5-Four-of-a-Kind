using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TV5_VolunteerEventMgmtApp.Data;
using TV5_VolunteerEventMgmtApp.Models;
using TV5_VolunteerEventMgmtApp.ViewModels;

namespace TV5_VolunteerEventMgmtApp.Controllers
{
    public class AttendanceSheetController : Controller
    {
        private readonly VolunteerEventMgmtAppDbContext _context;

        public AttendanceSheetController(VolunteerEventMgmtAppDbContext context)
        {
            _context = context;
        }

        // GET: AttendanceSheet
        public async Task<IActionResult> Index()
        {
            var volunteerEventMgmtAppDbContext = _context.AttendeesSheets
                .Include(a => a.Director)
                .Include(a => a.Location)
                .Include(a => a.Venue)
                .Include(a => a.Attendees);
            return View(await volunteerEventMgmtAppDbContext.ToListAsync());
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
                .Include(a => a.Venue)
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
			AttendanceSheet attendanceSheet = new AttendanceSheet();
			ViewData["DirectorId"] = new SelectList(_context.Directors, "ID", "FullName");
            ViewData["LocationID"] = new SelectList(_context.Locations, "ID", "City");
            ViewData["VenueId"] = new SelectList(_context.Set<Venue>(), "ID", "VenueName");
            PopulateSingerListBoxes(attendanceSheet);
			return View();
        }

        // POST: AttendanceSheet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DirectorId,Notes,StartTime,EndTime,LocationID,VenueId")] AttendanceSheet attendanceSheet,
			string[] selectedOptions)
        {
            UpdateAttendees(selectedOptions, attendanceSheet);
            if (ModelState.IsValid)
            {
                _context.Add(attendanceSheet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DirectorId"] = new SelectList(_context.Directors, "ID", "FullName", attendanceSheet.DirectorId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "ID", "City", attendanceSheet.LocationId);
            ViewData["VenueId"] = new SelectList(_context.Set<Venue>(), "ID", "VenueName", attendanceSheet.VenueId);
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
            ViewData["DirectorId"] = new SelectList(_context.Directors, "ID", "FullName", attendanceSheet.DirectorId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "ID", "City", attendanceSheet.LocationId);
            ViewData["VenueId"] = new SelectList(_context.Set<Venue>(), "ID", "VenueName", attendanceSheet.VenueId);
			PopulateSingerListBoxes(attendanceSheet);
			return View(attendanceSheet);
        }

        // POST: AttendanceSheet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string[] selectedOptions)
        {
			var attendanceSheetToUpdate = await _context.AttendeesSheets
				.Include(a => a.Attendees).ThenInclude(a => a.Singer)
				.FirstOrDefaultAsync(a => a.Id == id);

			if (id != attendanceSheetToUpdate.Id)
            {
                return NotFound();
			}

			UpdateAttendees(selectedOptions, attendanceSheetToUpdate);

			if (await TryUpdateModelAsync<AttendanceSheet>(attendanceSheetToUpdate, "",
		        a => a.DirectorId, a => a.Notes, a => a.StartTime, 
                a => a.EndTime, a => a.LocationId, a => a.VenueId))
			{
                try
                {
                    await _context.SaveChangesAsync();
					return RedirectToAction("Details", new { id = attendanceSheetToUpdate.Id });
				}
                catch (DbUpdateConcurrencyException)
                {
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
            ViewData["VenueId"] = new SelectList(_context.Set<Venue>(), "ID", "VenueName", attendanceSheetToUpdate.VenueId);
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
                .Include(a => a.Venue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendanceSheet == null)
            {
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
			var allSingers = _context.Singers;
			var currentSingersHS = new HashSet<int>(attendanceSheet.Attendees.Select(e => e.SingerId));

			var selected = new List<ListOptionVM>();
			var available = new List<ListOptionVM>();
			foreach (var c in allSingers)
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


		private bool AttendanceSheetExists(int id)
        {
            return _context.AttendeesSheets.Any(e => e.Id == id);
        }
    }
}
