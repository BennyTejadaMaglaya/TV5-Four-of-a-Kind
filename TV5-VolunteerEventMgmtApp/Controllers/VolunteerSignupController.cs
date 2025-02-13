using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using TV5_VolunteerEventMgmtApp.Data;
using TV5_VolunteerEventMgmtApp.Models;

namespace TV5_VolunteerEventMgmtApp.Controllers
{
    public class VolunteerSignupController : Controller
    {
        private readonly VolunteerEventMgmtAppDbContext _context;

        public VolunteerSignupController(VolunteerEventMgmtAppDbContext context)
        {
            _context = context;
        }

        // GET: VolunteerSignup
        public async Task<IActionResult> Index()
        {
            var volunteerEventMgmtAppDbContext = _context.VolunteerSignups.Include(v => v.VolunteerEvent);
            return View(await volunteerEventMgmtAppDbContext.ToListAsync());
        }

        // GET: VolunteerSignup/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerSignup = await _context.VolunteerSignups
                .Include(v => v.VolunteerEvent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (volunteerSignup == null)
            {
                return NotFound();
            }

            return View(volunteerSignup);
        }

        // GET: VolunteerSignup/Create
        public IActionResult Create()
        {
            ViewData["VolunteerEventId"] = new SelectList(_context.VolunteerEvents, "Id", "Description");
            return View();
        }

        // POST: VolunteerSignup/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StartTime,EndTime,TimeSlots,VolunteerEventId, VolunteerAttendees")] VolunteerSignup timeslot)
        {
            var eventTimes = await _context.VolunteerEvents
                                    .FindAsync(timeslot.VolunteerEventId);
            if (eventTimes == null) return NotFound();


            timeslot.StartTime = new DateTime(
              eventTimes.StartTime.Year,
              eventTimes.StartTime.Month,
              eventTimes.StartTime.Day,
              timeslot.StartTime.Hour,
              timeslot.StartTime.Minute,
              0
          );
            timeslot.EndTime = new DateTime(
                eventTimes.EndTime.Year,
                eventTimes.EndTime.Month,
                eventTimes.EndTime.Day,
                timeslot.EndTime.Hour,
                timeslot.EndTime.Minute,
                0
            );

            if (ModelState.IsValid)
            {
                _context.VolunteerSignups.Add(timeslot);
                await _context.SaveChangesAsync();
          
                return NoContent();
            }
            // If model invalid, re-render the create partial (with validation errors)
            return PartialView("_TimeSlotCreate", timeslot);
        }
        // POST: VolunteerSignup/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: TimeSlots/Edit/123
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,EndTime")] VolunteerSignup timeslot)
        {
     
            var originalTimeslot = await _context.VolunteerSignups.FindAsync(id);
            if (originalTimeslot == null)
            {
                return NotFound();
            }

            timeslot.StartTime = new DateTime(
                originalTimeslot.StartTime.Year,
                originalTimeslot.StartTime.Month,
                originalTimeslot.StartTime.Day,
                timeslot.StartTime.Hour,
                timeslot.StartTime.Minute,
                0
            );
            timeslot.EndTime = new DateTime(
                originalTimeslot.EndTime.Year,
                originalTimeslot.EndTime.Month,
                originalTimeslot.EndTime.Day,
                timeslot.EndTime.Hour,
                timeslot.EndTime.Minute,
                0
            );

            if (id != timeslot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timeslot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.VolunteerSignups.Any(ts => ts.Id == timeslot.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
                return PartialView("_TimeslotReadOnly", timeslot);
            }
            return PartialView("_TimeslotEdit", timeslot);
        }

        // GET: VolunteerSignup/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerSignup = await _context.VolunteerSignups
                .Include(v => v.VolunteerEvent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (volunteerSignup == null)
            {
                return NotFound();
            }

            return View(volunteerSignup);
        }

        // POST: VolunteerSignup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var volunteerSignup = await _context.VolunteerSignups.FindAsync(id);
            if (volunteerSignup != null)
            {
                _context.VolunteerSignups.Remove(volunteerSignup);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult GetReadOnlyPartial(int id)
        {
            var timeslot = _context.VolunteerSignups
                           .Include(ts => ts.VolunteerAttendees)
                           .ThenInclude(va => va.Volunteer)
                           .FirstOrDefault(ts => ts.Id == id);
            if (timeslot == null)
                return NotFound();
            return PartialView("_TimeSlotReadOnly", timeslot);
        }

        // GET: TimeSlots/GetEditPartial?id=123
        public async Task<IActionResult> GetEditPartial(int id)
        {
            var timeslot = await _context.VolunteerSignups
                            .Include(d => d.VolunteerAttendees)
                            .ThenInclude(d => d.Volunteer)
                            .FirstOrDefaultAsync(d => d.Id == id);
          
            return PartialView("_TimeSlotEdit", timeslot);
        }

        [HttpGet]
        public async Task<IActionResult> CreatePartial(int eventId)
        {

            var targetEvent = await _context.VolunteerEvents
                              
                                .FirstOrDefaultAsync(d => d.Id == eventId);

       
            if (targetEvent == null)
            {
                return NotFound();
            }
            var newTimeslot = new VolunteerSignup
            {
                VolunteerEventId = eventId,
                VolunteerEvent = targetEvent,
                StartTime = targetEvent.StartTime,
                EndTime = targetEvent.EndTime,
                TimeSlots = 0,
                VolunteerAttendees = new HashSet<VolunteerAttendee>()
        };

            return PartialView("_TimeslotCreate", newTimeslot);
        }

        [HttpGet]
        public async Task<IActionResult> GetTimeslotsPartial(int eventId)
        {
            // Load timeslots for this event, including volunteer attendees if needed.
            var timeslots = await _context.VolunteerSignups
                                .Include(ts => ts.VolunteerAttendees)
                                .ThenInclude(va => va.Volunteer)
                                .Where(ts => ts.VolunteerEventId == eventId)
                                .ToListAsync();
            // Return a partial view that renders the timeslot list.
            return PartialView("_TimeSlots", timeslots);
        }

        private bool VolunteerSignupExists(int id)
        {
            return _context.VolunteerSignups.Any(e => e.Id == id);
        }
    }
}
