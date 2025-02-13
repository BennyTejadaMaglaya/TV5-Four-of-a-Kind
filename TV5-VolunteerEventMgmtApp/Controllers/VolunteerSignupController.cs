﻿using System;
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
        public async Task<IActionResult> Create([Bind("Id,StartTime,EndTime,TimeSlots,VolunteerEventId")] VolunteerSignup volunteerSignup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(volunteerSignup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VolunteerEventId"] = new SelectList(_context.VolunteerEvents, "Id", "Description", volunteerSignup.VolunteerEventId);
            return View(volunteerSignup);
        }

        // GET: VolunteerSignup/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteerSignup = await _context.VolunteerSignups.FindAsync(id);
            if (volunteerSignup == null)
            {
                return NotFound();
            }
            ViewData["VolunteerEventId"] = new SelectList(_context.VolunteerEvents, "Id", "Description", volunteerSignup.VolunteerEventId);
            return View(volunteerSignup);
        }

        // POST: VolunteerSignup/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: TimeSlots/Edit/123
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,ArrivalTime,DepartureTime")] VolunteerSignup volunteerSignup)
        {
            if (id != volunteerSignup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteerSignup);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.VolunteerSignups.Any(ts => ts.Id == volunteerSignup.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                // Return the updated read-only partial view
                var updatedTimeslot = _context.VolunteerSignups
                    .Include(ts => ts.VolunteerAttendees)
                    .ThenInclude(va => va.Volunteer)
                    .FirstOrDefault(ts => ts.Id == volunteerSignup.Id);
                return PartialView("_TimeslotReadOnly", updatedTimeslot);
            }
            return PartialView("_TimeslotEdit", volunteerSignup);
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

        private bool VolunteerSignupExists(int id)
        {
            return _context.VolunteerSignups.Any(e => e.Id == id);
        }
    }
}
