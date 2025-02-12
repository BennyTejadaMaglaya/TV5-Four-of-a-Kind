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
    [Route("[controller]")]
    public class VolunteerController : Controller
    {
        private readonly VolunteerEventMgmtAppDbContext _context;

        public VolunteerController(VolunteerEventMgmtAppDbContext context)
        {
            _context = context;
        }

        // GET: Volunteer
        [HttpGet, Route("")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Volunteers.ToListAsync());
        }

        // GET: Volunteer/Details/5
        [HttpPost, Route("details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (volunteer == null)
            {
                return NotFound();
            }

            return View(volunteer);
        }

        // GET: Volunteer/Create
        [HttpGet, Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Volunteer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("create")]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,PhoneNumber,EmailAddress,JoinDate,TimesLate,numShifts,IsActive,IsConfirmed")] Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(volunteer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(volunteer);
        }

        // GET: Volunteer/Edit/5
        [HttpGet, Route("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer == null)
            {
                return NotFound();
            }
            return View(volunteer);
        }

        // POST: Volunteer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost, Route("edit/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,PhoneNumber,EmailAddress,JoinDate,TimesLate,numShifts,IsActive,IsConfirmed")] Volunteer volunteer)
        {
            if (id != volunteer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerExists(volunteer.Id))
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
            return View(volunteer);
        }

        // GET: Volunteer/Delete/5
        [HttpGet, Route("delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (volunteer == null)
            {
                return NotFound();
            }

            return View(volunteer);
        }

        // POST: Volunteer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer != null)
            {
                _context.Volunteers.Remove(volunteer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, Route("ConfirmVolunteer/{id}")]
        public async Task<IActionResult> ConfirmVolunteer(int id)
        {
            var volunteer = await _context.Volunteers.FirstOrDefaultAsync(v => v.Id == id);
            if(volunteer == null)
            {
                return NotFound("The volunteer with the id " + id + " doesn't exist. (Maybe deleted?)");
            }



            return Ok();
        }

        [HttpPost, Route("DenyVolunteer/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DenyVolunteer(int id)
        {
            var volunteer = await _context.Volunteers.FirstOrDefaultAsync(v => v.Id == id);

            if (volunteer == null)
            {
                return NotFound("The volunteer with the id " + id + " doesn't exist. (Maybe deleted?)");
            }

            return Ok();
        }

        private bool VolunteerExists(int id)
        {
            return _context.Volunteers.Any(e => e.Id == id);
        }
    }
}
