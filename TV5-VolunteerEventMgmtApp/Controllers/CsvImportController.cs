using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TV5_VolunteerEventMgmtApp.Data;
using TV5_VolunteerEventMgmtApp.Models;
using TV5_VolunteerEventMgmtApp.Services;

namespace TV5_VolunteerEventMgmtApp.Controllers
{
    [Route("[controller]")]
    public class CsvImportController : Controller
    {
        private readonly CsvService _csvService;
        private readonly VolunteerEventMgmtAppDbContext _context;
        public CsvImportController(CsvService csvService, VolunteerEventMgmtAppDbContext ctx)
        {
            _csvService = csvService;
            _context = ctx;
        }

        [HttpGet]
        public IActionResult Index()
        {

            ViewBag.Locations = LocationSelectList();
            return View(new List<SingerCsvUpload>());
        }

        [HttpPost]

        public async Task<IActionResult> Index(int locationId, IFormFile csvFile)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(c => c.ID == locationId);
            if (location == null)
            {
                return NotFound("There is no location with id " + locationId);
            }

            ViewBag.Locations = LocationSelectList(selected: locationId);
            Console.WriteLine(csvFile.ToString(), csvFile.Length);
            if (csvFile != null && csvFile.Length > 0)
            {
                Console.WriteLine("Here1");
                using (var stream = csvFile.OpenReadStream())
                {
                    try
                    {
                        var users = _csvService.ReadSingerCsvFile(stream).ToList();

                        UploadSingers(users, location);
                        return View(users);
                    }
                    catch (ApplicationException ex)
                    { // invalid csv errors
                        Console.WriteLine(ex.Message);
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please select a valid CSV file.");
            }
            return View(new List<SingerCsvUpload>());
        }

        private void UploadSingers(List<SingerCsvUpload> singers, Location location)
        {
            foreach (var singer in singers)
            {
                if (!singer.IsValid())
                {
                    Console.WriteLine("Invalid singer");
                    // either we stop entire operation or just skip this one not sure yet
                    continue; // for now we just skip
                }

                var singerObj = new Singer
                {
                    LastName = singer.LastName,
                    FirstName = singer.FirstName,
                    Email = singer.Email,
                    Phone = singer.PhoneNumber,
                    DOB = singer.DateOfBirth()
                };
                var lo = new SingerLocation { Location = location, Singer = singerObj };
                try
                {
                    _context.Add(singerObj);
                    _context.Add(lo);
                    _context.SaveChanges();
                    
                    
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    _context.Remove(singerObj);
                    _context.Remove(lo);
                }
            }

        }

        private SelectList LocationSelectList(bool activeOnly=true, int selected=-1)
        {
            return new SelectList(activeOnly ? _context.Locations.Where(s => s.IsActive) : _context.Locations, "ID", "City", selected);
        }
    }
}
