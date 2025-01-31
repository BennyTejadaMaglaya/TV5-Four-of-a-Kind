using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TV5_VolunteerEventMgmtApp.CustomControllers;
using TV5_VolunteerEventMgmtApp.Data;
using TV5_VolunteerEventMgmtApp.Models;
using TV5_VolunteerEventMgmtApp.Utilities;
using TV5_VolunteerEventMgmtApp.ViewModels;

namespace TV5_VolunteerEventMgmtApp.Controllers
{
    public class DirectorController : ElephantController
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
			// Filtering data
			ViewData["BtnBg"] = "btn-outline-dark";
			ViewData["BtnText"] = "Filters";
			int numberFilters = 0;

            var directors = _context
                .Directors
                .Include(d => d.DirectorLocations)
                .ThenInclude(s => s.Location)
                .Where(d => d.IsActive ==  true)
                .AsNoTracking();

			if (!string.IsNullOrEmpty(searchName))
            {
                directors = directors.Where(s => s.FirstName.ToLower().Contains(searchName.ToLower()) || s.LastName.ToLower().Contains(searchName.ToLower()));
				numberFilters++;
			}
			if (numberFilters != 0)
			{
				ViewData["BtnBg"] = "btn-dark";
				ViewData["BtnText"] = $"{numberFilters} Filter{(numberFilters > 1 ? "s" : "")} Applied";
				ViewData["ShowFilter"] = " show";
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
            Director director = new Director();
            PopulateAssignedLocations(director);
            return View();
        }

        // POST: Director/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,PhoneNumber,Email")] Director director, string[] selectedLocations)
        {
            try
            {
                if (selectedLocations != null)
                {
                    foreach (var location in selectedLocations)
                    {
                        var locationToAdd = new DirectorLocation { DirectorID = director.ID, LocationID = int.Parse(location) };
                        director.DirectorLocations.Add(locationToAdd);
                    };
                    ModelState.Remove("DirectorLocations");
                }
                
                
                if (ModelState.IsValid)
                {   
                    _context.Add(director);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new {director.ID});
                }
            }
            catch
            {
                // todo update when we get to concurrency
                ModelState.AddModelError("", "Error when creating this director");
            }
            PopulateAssignedLocations(director);
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
                .Include(d => d.DirectorLocations).ThenInclude(dl => dl.Location)
				.FirstOrDefaultAsync(l => l.ID == id);
            if (director == null)
            {
                return NotFound();
            }
            PopulateAssignedLocations(director);
            return View(director);
        }

        // POST: Director/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string[] selectedLocations)
        {
            var director = await _context.Directors
                .Include(d => d.DirectorLocations).ThenInclude(dl => dl.Location)
				.FirstOrDefaultAsync(d => d.ID == id);
            if(director == null)
            {
                return NotFound();
            }

            if(selectedLocations.Length == 0)
            {
                ModelState.AddModelError("DirectorLocations", "A director requires at least 1 location.");
                PopulateAssignedLocations(director);
                return View(director);
            }
            ModelState.Remove("DirectorLocations");
            UpdateDirectorLocation(selectedLocations, director);


            if (await TryUpdateModelAsync<Director>(director, "", d => d.FirstName, d=> d.LastName, d=> d.Email, d=> d.PhoneNumber))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new {director.ID});
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
                
            }
            PopulateAssignedLocations(director);
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
            var director = await _context.Directors
                .Include(d => d.AttendanceSheets)
                .FirstOrDefaultAsync(d => d.ID == id);

            if (director != null)
            {
                director.IsActive = false;
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

        private void PopulateAssignedLocations(Director director)
        {
            var allOptions = _context.Locations.Where(d => d.IsActive);
            var currentLocations = new HashSet<int>(director.DirectorLocations.Select(d => d.LocationID));
            var checkboxes = new List<CheckOptionVM>();
            foreach (var item in allOptions)
            {
                checkboxes.Add(new CheckOptionVM
                {
                    ID = item.ID,
                    DisplayText = item.City,
                    Assigned = currentLocations.Contains(item.ID)
                });
            }
            ViewData["AvailableLocations"] = checkboxes;
      
        }

        private SelectList LocationSelectList(bool availableOnly)
        {
            return new SelectList(availableOnly ? 
                _context.Locations.Where(l => l.IsActive) : _context.Locations, "ID", "City"); 
        }

        private void UpdateDirectorLocation(string[] selectedLocations, Director director)
        {
                      
            if(selectedLocations == null)
            {
                director.DirectorLocations = new List<DirectorLocation>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedLocations);
            var directorLocations = new HashSet<int>(director.DirectorLocations.Select(d => d.LocationID));
            foreach(var item in _context.Locations.Where(d => d.IsActive))
            {
                if (selectedOptionsHS.Contains(item.ID.ToString()))
                {
                    if (!directorLocations.Contains(item.ID))
                    {
                        director.DirectorLocations.Add(new DirectorLocation { DirectorID = director.ID, LocationID = item.ID });
                    }
                }
                else
                {
                    if (directorLocations.Contains(item.ID))
                    {
                        DirectorLocation locationToRemove = director.DirectorLocations.SingleOrDefault(d => d.LocationID == item.ID);
                        _context.Remove(locationToRemove);
                    }
                }
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
