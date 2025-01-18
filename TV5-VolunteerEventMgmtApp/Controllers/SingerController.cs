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
    public class SingerController : Controller
    {
        private readonly VolunteerEventMgmtAppDbContext _context;

        public SingerController(VolunteerEventMgmtAppDbContext context)
        {
            _context = context;
        }

        // GET: Singer
        public async Task<IActionResult> Index(
            string? actionButton,
            string? searchFirst ="",
            string? searchLast ="",
            string sortDirection = "asc", 
            string sortField = "First Name"
            )
        {
            string[] sortOptions = ["First Name", "Last Name", "Date of Birth"];
            var singers = _context.Singers.AsNoTracking();

            if (!string.IsNullOrEmpty(searchFirst))
            {
                singers = singers.Where(s => s.FirstName.ToLower().Contains(searchFirst.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchLast))
            {
                singers = singers.Where(s => s.LastName.ToLower().Contains(searchLast.ToLower()));
            }



            if (!String.IsNullOrEmpty(actionButton)) //Form Submitted!
            {
                //page = 1;//Reset page to start

                if (sortOptions.Contains(actionButton))//Change of sort is requested
                {
                    if (actionButton == sortField) //Reverse order on same field
                    {
                        sortDirection = sortDirection == "asc" ? "desc" : "asc";
                    }
                    sortField = actionButton;//Sort by the button clicked
                }
            }



            if (sortField == "First Name")
            {
               singers = sortDirection == "asc" ? 
                    singers.OrderByDescending(s => s.FirstName) : singers.OrderBy(s => s.FirstName);
            }
            if (sortField == "Last Name")
            {
                singers = sortDirection == "asc" ?
                     singers.OrderByDescending(s => s.LastName) : singers.OrderBy(s => s.LastName);
            }

            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;
            ViewData["searchFirst"] = searchFirst;
            ViewData["searchLast"] = searchLast;

            return View(await singers.ToListAsync());
        }

        // GET: Singer/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Singer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Singer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,DOB,Email,Phone")] Singer singer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(singer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            { // todo whenever we add concurrency controls update this
                ModelState.AddModelError("", "There was an error with your request ");
            }
            return View(singer);
        }

        // GET: Singer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var singer = await _context.Singers.FindAsync(id);
            if (singer == null)
            {
                return NotFound();
            }
            return View(singer);
        }

        // POST: Singer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            var singer = await _context.Singers.FirstOrDefaultAsync(s => s.Id == id);
            if (singer == null) 
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Singer>(singer, "", c=> c.FirstName, c=> c.LastName, c=> c.Email, c=> c.Phone))
            {
                try
                {
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
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
                _context.Singers.Remove(singer);
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
    }
}
