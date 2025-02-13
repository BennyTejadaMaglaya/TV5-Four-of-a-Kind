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
        [HttpGet, ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Volunteers.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("FirstName,LastName,PhoneNumber,EmailAddress,IsActive,IsConfirmed")] Volunteer volunteer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(volunteer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException e) 
            { 
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
        [ValidateAntiForgeryToken]
        [HttpPost, Route("edit/{id}"), ActionName("Edit")]
        public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,PhoneNumber,EmailAddress,IsActive,IsConfirmed")] Volunteer volunteer)
        {
            Console.WriteLine("WOW@!");
            if (id != volunteer.Id)
            {
                return NotFound();
            }
            Console.WriteLine(ModelState.IsValid);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerExists(volunteer.Id))
                    {
                        return View(volunteer);
                    }
                    else
                    {
                        throw;
                    }
                }
                catch(DbUpdateException e)
                {

                }
             
            }
            return RedirectToPage("/volunteer");
        }

        [HttpPost, Route("update/{id}")]
        public async Task<IActionResult> EditVolunteer(int id, [FromBody] UpdateVolunteer volunteer)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var v= await _context.Volunteers.FirstOrDefaultAsync(s => s.Id == id);
            if(v == null)
            {
                return NotFound();
            }
            v.FirstName= volunteer.FirstName;
            v.LastName= volunteer.LastName;
            v.PhoneNumber= volunteer.PhoneNumber;
            v.EmailAddress = volunteer.Email;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                return BadRequest();
            }
            catch(DbUpdateException e)
            {
                return BadRequest();
            }
            Console.WriteLine(ModelState.IsValid);
            Console.WriteLine(volunteer.FirstName);
            Console.WriteLine("WOW@!");

            return Ok();
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
                try
                { 

                    _context.Volunteers.Remove(volunteer);
                }
                catch(DbUpdateException e) 
                {

                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // these endpoints just change the confirmation status of a volunteer and can be reused
        [HttpPost, Route("ConfirmVolunteer/{id}")]
        public async Task<IActionResult> ConfirmVolunteer(int id)
        {
            var volunteer = await _context.Volunteers.FirstOrDefaultAsync(v => v.Id == id);
            if(volunteer == null)
            {
                return NotFound("The volunteer with the id " + id + " doesn't exist. (Maybe deleted?)");
            }

            try
            {
                volunteer.IsConfirmed = true;
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



            return Ok();
        }

        // these endpoints just change the confirmation status of a volunteer and can be reused
        [HttpPost, Route("DenyVolunteer/{id}")]
        public async Task<IActionResult> DenyVolunteer(int id)
        {
            var volunteer = await _context.Volunteers.FirstOrDefaultAsync(v => v.Id == id);
            Console.WriteLine("WOW");

            if (volunteer == null)
            {
                return NotFound("The volunteer with the id " + id + " doesn't exist. (Maybe deleted?)");
            }

            try
            {
                volunteer.IsConfirmed = false;
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }


            return Ok();
        }

        private bool VolunteerExists(int id)
        {
            return _context.Volunteers.Any(e => e.Id == id);
        }
    }
}
