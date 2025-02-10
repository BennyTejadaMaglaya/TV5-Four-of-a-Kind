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
    public class VolunteerAttendeesController : Controller
    {
        private readonly VolunteerEventMgmtAppDbContext _context;

        public VolunteerAttendeesController(VolunteerEventMgmtAppDbContext context)
        {
            _context = context;
        }

        // GET: VolunteerAttendees
        public async Task<IActionResult> Index()
        {
            var volunteerEventMgmtAppDbContext = _context.VolunteerAttendees.Include(v => v.Volunteer).Include(v => v.VolunteerSignup);
            return View(await volunteerEventMgmtAppDbContext.ToListAsync());
        }

        // GET: VolunteerAttendees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerAttendee = await _context.VolunteerAttendees
                .Include(v => v.Volunteer)
                .Include(v => v.VolunteerSignup)
                .FirstOrDefaultAsync(m => m.VolunteerSignupId == id);
            if (volunteerAttendee == null)
            {
                return NotFound();
            }

            return View(volunteerAttendee);
        }

        // GET: VolunteerAttendees/Create
        public IActionResult Create()
        {
            ViewData["VolunteerId"] = new SelectList(_context.Volunteers, "Id", "FirstName");
            ViewData["VolunteerSignupId"] = new SelectList(_context.VolunteerSignups, "Id", "Id");
            return View();
        }

        // POST: VolunteerAttendees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VolunteerId,VolunteerSignupId,ArrivalTime,DepartureTime")] VolunteerAttendee volunteerAttendee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(volunteerAttendee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VolunteerId"] = new SelectList(_context.Volunteers, "Id", "FirstName", volunteerAttendee.VolunteerId);
            ViewData["VolunteerSignupId"] = new SelectList(_context.VolunteerSignups, "Id", "Id", volunteerAttendee.VolunteerSignupId);
            return View(volunteerAttendee);
        }

        [HttpPost]
    
        public async Task<IActionResult> quickCreate([FromBody] VolunteerAttendeeDTO dto)
        {
            VolunteerAttendee newAttendee = new VolunteerAttendee
            {
                VolunteerId = dto.volunteerId,
                VolunteerSignupId = dto.volunteerSignupId
            };

            _context.VolunteerAttendees.Add(newAttendee);
            await _context.SaveChangesAsync();
            return NoContent();
        }

		[HttpPost]
		public async Task<IActionResult> RemoveVolunteer([FromBody] VolunteerAttendeeDTO dto)
		{
			// e.g. remove from _context.VolunteerAttendees
			var attendee = await _context.VolunteerAttendees
				.FirstOrDefaultAsync(a => a.VolunteerSignupId == dto.volunteerSignupId
									   && a.VolunteerId == dto.volunteerId);

			if (attendee == null)
			{
				return NotFound();
			}

			_context.VolunteerAttendees.Remove(attendee);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		// GET: VolunteerAttendees/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerAttendee = await _context.VolunteerAttendees.FindAsync(id);
            if (volunteerAttendee == null)
            {
                return NotFound();
            }
            ViewData["VolunteerId"] = new SelectList(_context.Volunteers, "Id", "FirstName", volunteerAttendee.VolunteerId);
            ViewData["VolunteerSignupId"] = new SelectList(_context.VolunteerSignups, "Id", "Id", volunteerAttendee.VolunteerSignupId);
            return View(volunteerAttendee);
        }

        // POST: VolunteerAttendees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VolunteerId,VolunteerSignupId,ArrivalTime,DepartureTime")] VolunteerAttendee volunteerAttendee)
        {
            if (id != volunteerAttendee.VolunteerSignupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteerAttendee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerAttendeeExists(volunteerAttendee.VolunteerSignupId))
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
            ViewData["VolunteerId"] = new SelectList(_context.Volunteers, "Id", "FirstName", volunteerAttendee.VolunteerId);
            ViewData["VolunteerSignupId"] = new SelectList(_context.VolunteerSignups, "Id", "Id", volunteerAttendee.VolunteerSignupId);
            return View(volunteerAttendee);
        }

        // GET: VolunteerAttendees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerAttendee = await _context.VolunteerAttendees
                .Include(v => v.Volunteer)
                .Include(v => v.VolunteerSignup)
                .FirstOrDefaultAsync(m => m.VolunteerSignupId == id);
            if (volunteerAttendee == null)
            {
                return NotFound();
            }

            return View(volunteerAttendee);
        }

        // POST: VolunteerAttendees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var volunteerAttendee = await _context.VolunteerAttendees.FindAsync(id);
            if (volunteerAttendee != null)
            {
                _context.VolunteerAttendees.Remove(volunteerAttendee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VolunteerAttendeeExists(int id)
        {
            return _context.VolunteerAttendees.Any(e => e.VolunteerSignupId == id);
        }
    }
}
