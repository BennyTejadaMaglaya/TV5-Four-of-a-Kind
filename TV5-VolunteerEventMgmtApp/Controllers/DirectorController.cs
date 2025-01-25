using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TV5_VolunteerEventMgmtApp.Data;
using TV5_VolunteerEventMgmtApp.Models;
using TV5_VolunteerEventMgmtApp.Utilities;

namespace TV5_VolunteerEventMgmtApp.Controllers
{
    public class DirectorController : Controller
    {
        private readonly VolunteerEventMgmtAppDbContext _context;

        public DirectorController(VolunteerEventMgmtAppDbContext context)
        {
            _context = context;
        }

        // GET: Director
        public async Task<IActionResult> Index(string? actionButton,
                                    string? searchName,
                                    string sortDirection = "asc",
                                    string sortField = "First Name")
        {
            var directors = _context
                .Directors
                .Include(d => d.DirectorLocations)
                .ThenInclude(s => s.Location)
                .AsNoTracking();

            if(!string.IsNullOrEmpty(searchName))
            {
                directors = directors.Where(s => s.FirstName.ToLower().Contains(searchName.ToLower()) || s.LastName.ToLower().Contains(searchName.ToLower()));
            }
            SortUtilities.SwapSortDirection(ref sortField, ref sortDirection, ["First Name", "Last Name", "Location"], actionButton);
            SortDirectors(ref directors, sortField, sortDirection);
            PopulateSortFields(sortDirection, sortField);
            ViewData["searchName"] = searchName;

            return View(await directors.ToListAsync());
        }

        // GET: Director/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var director = await _context.Directors
                .Include(d => d.DirectorLocations)
                .ThenInclude(d => d.Location)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (director == null)
            {
                return NotFound();
            }

            return View(director);
        }

        // GET: Director/Create
        public IActionResult Create()
        {
            ViewBag.availableLocations = LocationSelectList(true);
            return View();
        }

        // POST: Director/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,PhoneNumber,Email")] Director director, int location =-1)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Console.WriteLine("ms");
                    if(location > -1)
                    {
                        director.DirectorLocations.Add(new DirectorLocation { LocationID = location });
                    }
                    _context.Add(director);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                // todo update when we get to concurrency
                ModelState.AddModelError("", "Error when creating this director");
            }
            return View(director);
        }

        // GET: Director/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var director = await _context.Directors
                .Include(d => d.DirectorLocations).ThenInclude(dl => dl.LocationID)
				.FirstOrDefaultAsync(l => l.ID == id);
            if (director == null)
            {
                return NotFound();
            }
            Console.WriteLine(director.DirectorLocations.Count);

            
            
            ViewBag.availableLocations = director.DirectorLocations.Count > 0 ?
                LocationSelectList(true, director.DirectorLocations.First().LocationID) : LocationSelectList(true);
            return View(director);
        }

        // POST: Director/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int location =-1)
        {
            var director = await _context.Directors
                .Include(d => d.DirectorLocations).ThenInclude(dl => dl.LocationID)
				.FirstOrDefaultAsync(d => d.ID == id);
            if(director == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync(director, "", d => d.FirstName, d=> d.LastName, d=> d.Email, d=> d.PhoneNumber))
            {
                try
                {
                    UpdateDirectorLocation(location, director);
                  
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DirectorExists(director.ID))
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

            return View(director);
        }

        // GET: Director/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var director = await _context.Directors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (director == null)
            {
                return NotFound();
            }

            return View(director);
        }

        // POST: Director/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var director = await _context.Directors.FindAsync(id);
            if (director != null)
            {
                _context.Directors.Remove(director);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                ModelState.AddModelError("", StaticMessages.PlaceholderError);
                return View(director);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DirectorExists(int id)
        {
            return _context.Directors.Any(e => e.ID == id);
        }

        private SelectList LocationSelectList(bool availableOnly, int selected =-1)
        {
            return new SelectList(availableOnly ? 
                _context.Locations.Where(l => l.IsActive) : _context.Locations, "ID", "City", selected); 
        }

        private void UpdateDirectorLocation(int location, Director director)
        {
            if (location == -1)
            { // if we have multiple directors in the future we just need to change this to filter out the ids
                // this is messy for now but if the issue arises its in place already.
                Console.WriteLine("Dir = 0");
                director.DirectorLocations = new List<DirectorLocation>();
            }
            else
            {
                var l = director.DirectorLocations.FirstOrDefault();
                if (l?.DirectorID != location)
                {
                    if (l != null)
                    {
                        _context.DirectorLocations.Remove(director.DirectorLocations.First());
                    }

                    
                }
                director.DirectorLocations =
                        new List<DirectorLocation>() { new DirectorLocation { DirectorID = director.ID, LocationID = location } };

            }
        }

        private void SortDirectors(ref IQueryable<Director> sheets, string sortField, string sortDirection)
        {
            if (sortField == "First Name")
            {
                sheets = sortDirection == "asc" ?
                    sheets.OrderByDescending(s => s.FirstName.ToLower()) : sheets.OrderBy(s => s.FirstName.ToLower());
            }

            if (sortField == "Last Name")
            {
                sheets = sortDirection == "asc" ?
                    sheets.OrderByDescending(s => s.LastName.ToLower()) : sheets.OrderBy(s => s.LastName.ToLower());
            }
            if(sortField == "Location")
            {
                sheets = sortDirection == "asc" ?
                    sheets.OrderByDescending(s => s.DirectorLocations.FirstOrDefault().Location.City) : sheets.OrderBy(s => s.DirectorLocations.FirstOrDefault().Location.City);
            }
        }

        private void PopulateSortFields(string sortDirection, string sortField)
        {
            ViewData["sortDirection"] = sortDirection;
            ViewData["sortField"] = sortField;
        }
    }
}
