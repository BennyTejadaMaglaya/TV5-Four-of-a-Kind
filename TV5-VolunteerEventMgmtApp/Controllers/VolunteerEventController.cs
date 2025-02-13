using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TV5_VolunteerEventMgmtApp.Data;
using TV5_VolunteerEventMgmtApp.Models;
using TV5_VolunteerEventMgmtApp.ViewModels;

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
            var volunteerEventMgmtAppDbContext = _context.VolunteerEvents
                .Include(v => v.Location)
                .Include(v => v.Venue)
                .Include(d => d.TimeSlots).ThenInclude(d => d.VolunteerAttendees).ThenInclude(d => d.Volunteer);
            return View(await volunteerEventMgmtAppDbContext.ToListAsync());
        }

        public async Task<IActionResult> GetVolunteersByLocation(int locationId)
        {
            var volunteersByLoc = await _context.Volunteers
                .Include(d => d.VolunteerLocations)
                .Where(d => d.VolunteerLocations.Any(d1 => d1.LocationId == locationId) && d.IsActive && d.IsConfirmed)
                .OrderByDescending(d => d.TimesLate).ThenBy(d => d.LastName).ThenBy(d => d.FirstName)
                .ToListAsync();


            return PartialView("_VolunteersList", volunteersByLoc);
        }

        [HttpGet]
        public async Task<IActionResult> GetTimeSlots(int eventId)
        {
            var TimeSlots = await _context.VolunteerSignups
                .Include(d => d.VolunteerAttendees).ThenInclude(d => d.Volunteer)
                .Where(d => d.VolunteerEventId == eventId)
                .ToListAsync();

            return PartialView("_TimeSlotReadOnly", TimeSlots);
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
        public async Task<IActionResult> Edit(int? id, bool isEdit = true)
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

            ViewBag.IsEditMode = isEdit;

            ViewData["LocationId"] = new SelectList(_context.Locations, "ID", "City", volunteerEvent.LocationId);
            ViewData["VenueId"] = new SelectList(_context.Venues.Where(d => d.LocationId == volunteerEvent.LocationId), "ID", "VenueName", volunteerEvent.VenueId);
            return View(volunteerEvent);
        }

        // POST: VolunteerEvent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VolunteerEvent volunteerEvent)
        {
            var eventToUpdate = await _context.VolunteerEvents
                .FirstOrDefaultAsync(d => d.Id == id);

            if (eventToUpdate == null)
            {
                return NotFound();
            }

            if(await TryUpdateModelAsync<VolunteerEvent>(eventToUpdate, "",
                d => d.Title, 
                d => d.IsActive,
                d => d.StartTime,
                d => d.EndTime, 
                d => d.Description,
                d => d.VenueId,
                d => d.LocationId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Event: {volunteerEvent.Title} saved!";
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)//This is a Transaction in the Database!
                {
                    TempData["FailMessage"] = "Unable to save event.";
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. " +
                        "Try again, and if the problem persists, see your system administrator.");
                }
                catch (DbUpdateConcurrencyException ex)// Added for concurrency
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (VolunteerEvent)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        TempData["FailMessage"] = "Unable to save event.";
                        ModelState.AddModelError("",
                            "Unable to save changes. The Event was deleted by another user.");
                    }
                 
                }
                catch (DbUpdateException dex)
                {
                    string message = dex.GetBaseException().Message;
                    TempData["FailMessage"] = "Unable to save event.";
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    
                }
            }
          
            ViewData["LocationId"] = new SelectList(_context.Locations, "ID", "City", volunteerEvent.LocationId);
            ViewData["VenueId"] = new SelectList(_context.Venues.Where(d => d.LocationId == volunteerEvent.LocationId), "ID", "VenueName", volunteerEvent.VenueId);
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

        [HttpGet]
        public IActionResult GetReadOnlyPartial(int id)
        {
            var volunteerEvent = _context.VolunteerEvents
                .Include(e => e.Venue)
                .Include(d => d.Location)                
                .FirstOrDefault(e => e.Id == id);

            if (volunteerEvent == null)
                return NotFound();

            return PartialView("_EventCardReadOnly", volunteerEvent);
        }

        [HttpGet]
        public IActionResult GetEditPartial(int id)
        {


            /// using identity system check here what the user role is if its not admin return the read only partial.
            /// this will allow for volunteers to signup to the evnt and keep the same functionality
            var volunteerEvent = _context.VolunteerEvents
                .Include(e => e.Venue)
                .FirstOrDefault(e => e.Id == id);

            if (volunteerEvent == null)
                return NotFound();


            ViewData["VenueList"] = new SelectList(_context.Venues.Where(d => d.LocationId == volunteerEvent.LocationId), "ID", "VenueName", volunteerEvent.VenueId);

            return PartialView("_EventCardEdit", volunteerEvent);
        }
    }
}
