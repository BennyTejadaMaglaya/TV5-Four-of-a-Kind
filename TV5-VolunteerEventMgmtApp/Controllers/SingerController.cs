using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TV5_VolunteerEventMgmtApp.Data;
using TV5_VolunteerEventMgmtApp.Models;
using TV5_VolunteerEventMgmtApp.ViewModels;
using TV5_VolunteerEventMgmtApp.Utilities;
using TV5_VolunteerEventMgmtApp.CustomControllers;
using System.IO;

namespace TV5_VolunteerEventMgmtApp.Controllers
{
	public class SingerController : ElephantController
	{
		private readonly VolunteerEventMgmtAppDbContext _context;

		public SingerController(VolunteerEventMgmtAppDbContext context)
		{
			_context = context;
		}

		// GET: Singer
		public async Task<IActionResult> Index(
			int? page,
			int? pageSizeID,
			string? actionButton,
			string? searchFirst = "",
			string? searchLast = "",
			string sortDirection = "asc",
			string sortField = "First Name"
			)
		{
			// List of sort options
			string[] sortOptions = ["First Name", "Last Name", "Date of Birth"];

			// Filtering data
			ViewData["BtnBg"] = "btn-outline-dark";
			ViewData["BtnText"] = "Filters";
			int numberFilters = 0;

			var singers = _context.Singers.Where(d => d.isActive == true).AsNoTracking();

			// Filters
			if (!string.IsNullOrEmpty(searchFirst))
			{
				singers = singers.Where(s => s.FirstName.ToLower().Contains(searchFirst.ToLower()));
				numberFilters++;
			}
			if (!string.IsNullOrEmpty(searchLast))
			{
				singers = singers.Where(s => s.LastName.ToLower().Contains(searchLast.ToLower()));
				numberFilters++;
			}
			if (numberFilters != 0)
			{
				ViewData["BtnBg"] = "btn-dark";
				ViewData["BtnText"] = $"{numberFilters} Filter{(numberFilters > 1 ? "s" : "")} Applied";
				ViewData["ShowFilter"] = " show";
			}

			// Check if there is a call for a change of filtering or sorting
			if (!String.IsNullOrEmpty(actionButton)) // Form Submitted
			{
				page = 1; // Reset page to start if filtering or sorting

				if (sortOptions.Contains(actionButton)) // Change of sort is requested
				{
					if (actionButton == sortField) // Reverse order on same field
					{
						sortDirection = sortDirection == "asc" ? "desc" : "asc";
					}
					else
					{
						if (actionButton == "Date of Birth")
						{
							sortDirection = "desc";
						}
						else
						{
							sortDirection = "asc";
						}
					}
					sortField = actionButton; // Sort by the button clicked
				}
			}

			// Sort by
			if (sortField == "First Name")
			{
				singers = sortDirection == "asc" 
					? singers.OrderBy(s => s.FirstName).ThenBy(s => s.LastName) 
					: singers.OrderByDescending(s => s.FirstName).ThenByDescending(s => s.LastName);
			}
			if (sortField == "Last Name")
			{
				singers = sortDirection == "asc" 
					? singers.OrderBy(s => s.LastName).ThenBy(s => s.FirstName)
					: singers.OrderByDescending(s => s.LastName).ThenByDescending(s => s.FirstName);
			}
			if (sortField == "Date of Birth")
			{
				singers = sortDirection == "asc"
					? singers.OrderBy(s => s.DOB).ThenByDescending(s => s.FirstName).ThenByDescending(s => s.LastName)
					: singers.OrderByDescending(s => s.DOB).ThenBy(s => s.FirstName).ThenBy(s => s.LastName);
			}

			// Set sort for next time
			ViewData["sortField"] = sortField;
			ViewData["searchFirst"] = searchFirst;
			ViewData["searchLast"] = searchLast;
			ViewData["sortDirection"] = sortDirection;

			// Handle Paging
			int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
			ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
			var pagedData = await PaginatedList<Singer>.CreateAsync(singers.AsNoTracking(), page ?? 1, pageSize);


			return View(pagedData);
		}

		// GET: Singer/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var singer = await _context.Singers
				.Include(s => s.SingerLocation)
				.ThenInclude(s => s.Location).Where(l => l.isActive == true)
				.FirstOrDefaultAsync(s => s.Id == id);

			if (singer == null)
			{
				return NotFound();
			}
		
			return View(singer);
		}

		// GET: Singer/Create
		public IActionResult Create()
		{
			Singer singer = new Singer();
			PopulateAssignedLocations(singer);
			
			return View();
		}

		// POST: Singer/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("FirstName,LastName,DOB,Email,Phone")] Singer singer, string[] selectedLocations)
		{
			try
			{

                if (selectedLocations != null)
                {
                    foreach (var location in selectedLocations)
                    {
                        var locationToAdd = new SingerLocation { SingerId = singer.Id, LocationId = int.Parse(location) };
                        singer.SingerLocation.Add(locationToAdd);
                    };
                }


                if (ModelState.IsValid)
				{
					_context.Add(singer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { singer.Id });
                }
					
				
            
			}
			catch
			{ // todo whenever we add concurrency controls update this
				ModelState.AddModelError("", "There was an error with your request ");
			}
            PopulateAssignedLocations(singer);
            return View(singer);
		}

		// GET: Singer/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var singer = await _context.Singers
				.Include(s => s.SingerLocation)
				.FirstOrDefaultAsync(s => s.Id == id);
			if (singer == null)
			{
				return NotFound();
			}
            PopulateAssignedLocations(singer);
            return View(singer);
		}

		// POST: Singer/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, string[] selectedLocations)
		{
			var singer = await _context.Singers.Include(s => s.SingerLocation).ThenInclude(d => d.Location).FirstOrDefaultAsync(s => s.Id == id);
			if (singer == null)
			{
				return NotFound();
			}

            if (selectedLocations.Length == 0)
            {
                ModelState.AddModelError("SingerLocation", "A director requires at least 1 location.");
                PopulateAssignedLocations(singer);
                return View(singer);
            }

            UpdateSingerLocation(selectedLocations, singer);

            if (await TryUpdateModelAsync<Singer>(singer, "", c => c.FirstName, c => c.LastName, c => c.Email, c => c.Phone))
			{
				try
				{
					

					await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { singer.Id });
                }
				catch (DbUpdateConcurrencyException)
				{
					if (!SingerExists(singer.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				
			}
            PopulateAssignedLocations(singer);
            return View(singer);
		}

		// GET: Singer/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var singer = await _context.Singers
				.FirstOrDefaultAsync(m => m.Id == id);
			if (singer == null)
			{
				return NotFound();
			}

			return View(singer);
		}

		// POST: Singer/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var singer = await _context.Singers.FindAsync(id);
			if (singer != null)
			{
				singer.isActive = false;
			}
			try
			{

				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			catch
			{ // todo update when concurrency comes 
				ModelState.AddModelError("", "Unable to delete this singer");
				return View(singer);
			}
		}

		private bool SingerExists(int id)
		{
			return _context.Singers.Any(e => e.Id == id);
		}

		[HttpGet]
		public JsonResult GetLocations(int? id)
		{
			return Json(LocationSelectList(id));
		}

		private SelectList LocationSelectList(int? selected = -1)
		{
			var d = _context.Locations.Select(d => new { d.ID, City = d.City });

			return new SelectList(d, "ID", "City", selected);
		}

		private void PopulateLocationSelect(bool availableOnly = true, int? selected=-1)
		{
            ViewBag.AvailableLocations = new SelectList(availableOnly ?
                _context.Locations.Where(l => l.IsActive) : _context.Locations, "ID", "City", selected);
        }

        private void PopulateAssignedLocations(Singer singer)
        {
            var allOptions = _context.Locations.Where(d => d.IsActive);
            var currentLocations = new HashSet<int>(singer.SingerLocation.Select(d => d.LocationId));
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


        private void UpdateSingerLocation(string[] selectedLocations, Singer singer)
        {

            if (selectedLocations == null)
            {
                singer.SingerLocation = new List<SingerLocation>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedLocations);
            var singerLocations = new HashSet<int>(singer.SingerLocation.Select(d => d.LocationId));
            foreach (var item in _context.Locations.Where(d => d.IsActive))
            {
                if (selectedOptionsHS.Contains(item.ID.ToString()))
                {
                    if (!singerLocations.Contains(item.ID))
                    {
                        singer.SingerLocation.Add(new SingerLocation { SingerId = singer.Id, LocationId = item.ID });
                    }
                }
                else
                {
                    if (singerLocations.Contains(item.ID))
                    {
                        SingerLocation locationToRemove = singer.SingerLocation.SingleOrDefault(d => d.LocationId == item.ID);
                        _context.Remove(locationToRemove);
                    }
                }
            }

        }
    }
}
