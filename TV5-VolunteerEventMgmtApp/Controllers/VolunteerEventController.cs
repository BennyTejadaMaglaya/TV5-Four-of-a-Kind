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
    public class VolunteerEventController : Controller
    {
        private readonly VolunteerEventMgmtAppDbContext _context;

        public VolunteerEventController(VolunteerEventMgmtAppDbContext context)
        {
            _context = context;
        }

        // GET: VolunteerEvent
        public async Task<IActionResult> Index()
        {
            var volunteerEventMgmtAppDbContext = _context.VolunteerEvents.Include(v => v.Location).Include(v => v.Venue);
            return View(await volunteerEventMgmtAppDbContext.ToListAsync());
        }

        // GET: VolunteerEvent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerEvent = await _context.VolunteerEvents
                .Include(v => v.Location)
                .Include(v => v.Venue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (volunteerEvent == null)
            {
                return NotFound();
            }

            return View(volunteerEvent);
        }

        // GET: VolunteerEvent/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Locations, "ID", "City");
            ViewData["VenueId"] = new SelectList(_context.Venues, "ID", "Address");
            return View();
        }

        // POST: VolunteerEvent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,IsActive,StartTime,EndTime,Description,LocationId,VenueId")] VolunteerEvent volunteerEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(volunteerEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "ID", "City", volunteerEvent.LocationId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "ID", "Address", volunteerEvent.VenueId);
            return View(volunteerEvent);
        }

        // GET: VolunteerEvent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerEvent = await _context.VolunteerEvents.FindAsync(id);
            if (volunteerEvent == null)
            {
                return NotFound();
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "ID", "City", volunteerEvent.LocationId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "ID", "Address", volunteerEvent.VenueId);
            return View(volunteerEvent);
        }

        // POST: VolunteerEvent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,IsActive,StartTime,EndTime,Description,LocationId,VenueId")] VolunteerEvent volunteerEvent)
        {
            if (id != volunteerEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteerEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerEventExists(volunteerEvent.Id))
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
            ViewData["LocationId"] = new SelectList(_context.Locations, "ID", "City", volunteerEvent.LocationId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "ID", "Address", volunteerEvent.VenueId);
            return View(volunteerEvent);
        }

        // GET: VolunteerEvent/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerEvent = await _context.VolunteerEvents
                .Include(v => v.Location)
                .Include(v => v.Venue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (volunteerEvent == null)
            {
                return NotFound();
            }

            return View(volunteerEvent);
        }

        // POST: VolunteerEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var volunteerEvent = await _context.VolunteerEvents.FindAsync(id);
            if (volunteerEvent != null)
            {
                _context.VolunteerEvents.Remove(volunteerEvent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VolunteerEventExists(int id)
        {
            return _context.VolunteerEvents.Any(e => e.Id == id);
        }
    }
}
