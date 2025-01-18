using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TV5_VolunteerEventMgmtApp.Data;
using TV5_VolunteerEventMgmtApp.Models;

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
            var volunteerEventMgmtAppDbContext = _context.AttendeesSheets.Include(a => a.Director).Include(a => a.Location).Include(a => a.Venue);
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
            ViewData["DirectorId"] = new SelectList(_context.Directors, "ID", "Email");
            ViewData["LocationID"] = new SelectList(_context.Locations, "ID", "City");
            ViewData["VenueId"] = new SelectList(_context.Set<Venue>(), "ID", "Address");
            return View();
        }

        // POST: AttendanceSheet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DirectorId,Notes,StartTime,EndTime,LocationID,VenueId")] AttendanceSheet attendanceSheet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendanceSheet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DirectorId"] = new SelectList(_context.Directors, "ID", "Email", attendanceSheet.DirectorId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "ID", "City", attendanceSheet.LocationId);
            ViewData["VenueId"] = new SelectList(_context.Set<Venue>(), "ID", "Address", attendanceSheet.VenueId);
            return View(attendanceSheet);
        }

        // GET: AttendanceSheet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendanceSheet = await _context.AttendeesSheets.FindAsync(id);
            if (attendanceSheet == null)
            {
                return NotFound();
            }
            ViewData["DirectorId"] = new SelectList(_context.Directors, "ID", "Email", attendanceSheet.DirectorId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "ID", "City", attendanceSheet.LocationId);
            ViewData["VenueId"] = new SelectList(_context.Set<Venue>(), "ID", "Address", attendanceSheet.VenueId);
            return View(attendanceSheet);
        }

        // POST: AttendanceSheet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DirectorId,Notes,StartTime,EndTime,LocationID,VenueId")] AttendanceSheet attendanceSheet)
        {
            if (id != attendanceSheet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendanceSheet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceSheetExists(attendanceSheet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DirectorId"] = new SelectList(_context.Directors, "ID", "Email", attendanceSheet.DirectorId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "ID", "City", attendanceSheet.LocationId);
            ViewData["VenueId"] = new SelectList(_context.Set<Venue>(), "ID", "Address", attendanceSheet.VenueId);
            return View(attendanceSheet);
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

        private bool AttendanceSheetExists(int id)
        {
            return _context.AttendeesSheets.Any(e => e.Id == id);
        }
    }
}
