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
    public class LocationController : Controller
    {
        private readonly VolunteerEventMgmtAppDbContext _context;

        public LocationController(VolunteerEventMgmtAppDbContext context)
        {
            _context = context;
        }

        // GET: Location
        public async Task<IActionResult> Index()
        {
            var locations = _context.Locations
                .Include(l => l.DirectorLocations)
                .Include(l => l.AttendanceSheets)
                .Include(l => l.Venues);

            // sort/filter by director
            // preview of this weeks attendence numbers? 
            
            return View(await locations.ToListAsync());
        }

        // GET: Location/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .FirstOrDefaultAsync(m => m.ID == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Location/Create
        public IActionResult Create()
        {
            ViewBag.availableDirectors = DirectorSelectList();
            return View();
        }

        // POST: Location/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("City,IsActive")] Location location, int director =-1)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(director > -1)
                    {
                        location.DirectorLocations.Add(new DirectorLocation { DirectorID=director });
                    }

                    _context.Add(location);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            { // todo update when we get to concurrency
                ModelState.AddModelError("", "Error when creating this location");
            }
            return View(location);
        }

        // GET: Location/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            

            var location = await _context.Locations.Include(l => l.DirectorLocations).FirstOrDefaultAsync(l => l.ID == id);
            if (location == null)
            {
                return NotFound();
            }
        
            ViewBag.availableDirectors = location.DirectorLocations.Count > 0 ? 
                DirectorSelectList(location.DirectorLocations.First().DirectorID) : DirectorSelectList();
            return View(location);
        }

        // POST: Location/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int director=-1)
        {
            var location = await _context.Locations
                .Include(l => l.DirectorLocations)
                .Where(i => i.ID == id)
                .FirstOrDefaultAsync();
            if (location == null)
            {
                return NotFound();
            }


            try
            {
               UpdateDirectorLocations(location, director);
                await _context.SaveChangesAsync();

                if (await TryUpdateModelAsync(location, "", d => d.City, d=> d.IsActive))
                {
                    
                    try
                    {
                        
                        await _context.SaveChangesAsync();
                        
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!LocationExists(location.ID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                   
                }
            } catch
            {

            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Location/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .FirstOrDefaultAsync(m => m.ID == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location != null)
            {
                _context.Locations.Remove(location);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.ID == id);
        }

        private SelectList DirectorSelectList(int selected = -1)
        { // Reminder that if theres a placeholder option in the select list
          // (which there is for this one) you should pass the id + 1 for the
          // selected index as 0 will always be the placeholder option
            var d = _context.Directors.Select(d => new { d.ID, Name = d.NameSummary() });

            return new SelectList( d, "ID", "Name", selected);
        }

        private void UpdateDirectorLocations(Location location, int director)
        {
            if (director == -1)
            { // if we have multiple directors in the future we just need to change this to filter out the id
                // this is messy for now but if the issue arises its in place already.
                location.DirectorLocations = new List<DirectorLocation>();
            }
            else
            {
                var l = location.DirectorLocations.FirstOrDefault();
                if (l?.DirectorID != director)
                {
                    if (l != null)
                    {
                        _context.DirectorLocations.Remove(location.DirectorLocations.First());
                    }

                    location.DirectorLocations = 
                        new List<DirectorLocation>() { new DirectorLocation { DirectorID = director, LocationID = location.ID } };
                }

            }

        }


    }
}
