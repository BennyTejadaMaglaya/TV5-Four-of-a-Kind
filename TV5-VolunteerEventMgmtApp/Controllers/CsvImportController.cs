using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.IO;
using TV5_VolunteerEventMgmtApp.Data;
using TV5_VolunteerEventMgmtApp.Models;
using TV5_VolunteerEventMgmtApp.Services;
using TV5_VolunteerEventMgmtApp.Utilities;
using TV5_VolunteerEventMgmtApp.Utilities.Csv;

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
                return Json(new
                {
                    Message = $"There is no location with the ID {locationId}",
                    Success = false
                });
            }

            ViewBag.Locations = LocationSelectList(selected: locationId);
            if (csvFile != null && csvFile.Length > 0)
            {
                Console.WriteLine("Here1");
                try
                {
                    var newSingers = ReadSingerCsv(csvFile);

                    //var csvErrors = ValidateAndAddSingers(newSingers, location, out var dbErrors, out var successCount);
                    var csvErrors = ValidateSingers(newSingers);
                    
                        var dbErrors = await AddSingersToDb(newSingers, location);
                        foreach (var error in dbErrors.Errors)
                        {
                            Console.WriteLine($"{error}");
                        ModelState.AddModelError("", error);
                        }
                    
                        
                    foreach(var error in csvErrors)
                    {
                       foreach(var e in error.Errors)
                        {
                            Console.WriteLine(e);
                            ModelState.AddModelError("", e);
                        }
                    }
                    return View(newSingers);
                }
                catch (ApplicationException ex)
                { // invalid csv errors
                    Console.WriteLine(ex.Message);
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please select a valid CSV file.");
            }
            //return Json(new
            //{
            //    Message = $"Please submit a valid CSV File.",
            //    IsValid = false
            //});
            return View(new List<SingerCsvUpload>());
        }

        [HttpPost, Route("UploadSingers")]
        public async Task<IActionResult> UploadSingers(IFormFile csvFile, int locationId)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(c => c.ID == locationId);
            if (location == null)
            {
                return NotFound("There is no location matching " + locationId);
            }
            List<SingerCsvUpload>? newSingers = null;
            try
            {
                newSingers = ReadSingerCsv(csvFile);
               
            }
            catch
            {
                // invalid csv File ( most likely headers )
                return BadRequest(StaticMessages.InvalidCSV);
            }

            if (newSingers != null)
            {
                var csvErrors = ValidateSingers(newSingers);
                var dbErrors = await AddSingersToDb(newSingers, location);
                return Json(new
                {
                    // CsvErrors type => List<CsvValidationResponse>
                    CsvErrors = csvErrors, // List<CsvValidationResponse> of errors with a value in the csv file (no first/lastname/DOB in a row
                    DbErrors = dbErrors.Errors, // List<String> of errors caused by the db (existing child being added, etc.)
                    dbErrors.SuccessCount, // number of records successfully added
                });
                
                
            }


            return BadRequest(StaticMessages.InvalidCSV);
        }

        private List<CsvValidationResponse> ValidateSingers(List<SingerCsvUpload> singers)
        {
            int lineCount = 1;
            var errors = new List<CsvValidationResponse>();
            var failedToAdd = new List<string>();
            foreach (var singer in singers)
            {
                var valid = singer.IsValid();
                if (!valid.IsValid)
                {
                    valid.LineOfRecord = lineCount;
                    errors.Add(valid);

                }

                lineCount++;

            }


            return errors;
        }

        //private List<CsvValidationResponse> ValidateAndAddSingers(
        //    List<SingerCsvUpload> singers,
        //    Location location,
        //    out List<string> dbErrors,
        //    out int successCount)
        //{
        //    // we first validate the entire file of singers making sure required info is there
        //    // return an error message if theres an invalid singer provide the line 
        //    var errors = ValidateSingers(singers);
        //    var failedToAdd = new List<string>();

        //    if (errors.Count < 1)
        //    {
        //        // then we can try and add them to the database
        //        try
        //        {
        //            var success = AddSingersToDb(singers, location, ref failedToAdd);
                    
        //        }
        //        catch(Exception e)
        //        {
        //            Console.WriteLine(e.GetBaseException().Message);
        //        }
        //    }
        //    dbErrors = failedToAdd;
        //    successCount = 0;
        //    return errors;

        //}

        private SelectList LocationSelectList(bool activeOnly = true, int selected = -1)
        {
            return new SelectList(activeOnly ? _context.Locations.Where(s => s.IsActive) :
                _context.Locations, "ID", "City", selected);
        }

        private async Task<DbErrorCollection> AddSingersToDb(
            List<SingerCsvUpload> singers,
            Location location
            )
        {
            var collection = new DbErrorCollection();
            //int success = 0;
            foreach (var singer in singers)
            {
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
                    await  _context.SaveChangesAsync();
                    collection.SuccessCount++;


                }
                catch (Exception e)
                {
                    if (e.GetBaseException().Message.Contains("UNIQUE"))
                    {
                        collection.Errors.Add($"Unable to add the singer {singerObj.FirstName} " +
                            $"{singerObj.LastName} as they already exist in the database.");
                    }
                    _context.Remove(singerObj);
                    //_context.Remove(lo);
                }

            }
            return collection;
        }

        private List<SingerCsvUpload> ReadSingerCsv(IFormFile csvFile)
        {
            if (csvFile != null && csvFile.Length > 0)
            {
                using (var stream = csvFile.OpenReadStream())
                {
                    var newSingers = _csvService.ReadSingerCsvFile(stream).ToList();
                    return newSingers;
                }
            }
            return [];

        }

        private List<DirectorCsvUpload> ReadDirectorCsv(IFormFile csvFile)
        {
            if (csvFile != null && csvFile.Length > 0)
            {
                using (var stream = csvFile.OpenReadStream())
                {
                    var newDirectors = _csvService.ReadDirectorCsvFile(stream).ToList();
                    return newDirectors;
                }
            }
            return [];

        }
    }
}
