using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TV5_VolunteerEventMgmtApp.Data;
using TV5_VolunteerEventMgmtApp.Models;
using TV5_VolunteerEventMgmtApp.Services;
using TV5_VolunteerEventMgmtApp.Utilities;
using TV5_VolunteerEventMgmtApp.Utilities.Csv;
using TV5_VolunteerEventMgmtApp.ViewModels;

namespace TV5_VolunteerEventMgmtApp.Controllers
{
    [Route("[controller]")]
    public class CsvImportController : Controller
    {
        private List<string> DefaultColors = new List<string> { "#4CAF50", "#2196F3", "#9C27B0", "#00BCD4", "#8BC34A", "#607D8B" };
        private readonly CsvService _csvService;
        private readonly VolunteerEventMgmtAppDbContext _context;
        public CsvImportController(CsvService csvService, VolunteerEventMgmtAppDbContext ctx)
        {
            _csvService = csvService;
            _context = ctx;
        }

        [HttpGet, Route("Singer")]
        public IActionResult Singer()
        {

            ViewBag.Locations = LocationSelectList();
            return View();
        }

        [HttpPost, Route("Singer")]

        public async Task<IActionResult> Singer(int locationId, IFormFile csvFile)
        { // if json is returned at least some of the records were created 
            // bad request means something went wrong (most likely invalid CSV)
            ViewBag.Locations = LocationSelectList();
            var location = await _context.Locations.FirstOrDefaultAsync(c => c.ID == locationId);
            if (location == null)
            {
                ModelState.AddModelError("", "There is no location with the ID " + locationId);
                return View(new CsvUploadVM<List<SingerCsvUpload>>
                {
                    Data = new List<SingerCsvUpload>()
                });
            }

            ViewBag.Locations = LocationSelectList(selected: locationId);
            if (csvFile != null && csvFile.Length > 0)
            {
                try
                {
                    var newSingers = ReadSingerCsv(csvFile);

                    //var csvErrors = ValidateAndAddSingers(newSingers, location, out var dbErrors, out var successCount);
                    var csvErrors = ValidateSingers(newSingers);

                    if (!csvErrors.All(s => s.IsValid))
                    {
                        MapModelStateErrors(csvErrors);
                        return View(new CsvUploadVM<List<SingerCsvUpload>>
                        {
                            Data = new List<SingerCsvUpload>()
                        });
                        
                        //return Json(new
                        //{
                        //    csvErrors
                        //});
                    }

                    var dbErrors = await AddSingersToDb(newSingers, location);

                    MapModelStateErrors(csvErrors);
                    MapModelStateErrors(dbErrors.Errors);
					//return Json(new
					//{
					//    csvErrors,
					//    dbErrors
					//});
					TempData["SuccessMessage"] = $"Singers added.";
					return View(new CsvUploadVM<List<SingerCsvUpload>>
                    {
                        Data = newSingers,
                        SuccessCount = dbErrors.SuccessCount
                    });
                }
                catch (ApplicationException ex)
                { // invalid csv errors
                    Console.WriteLine(ex.Message);
					TempData["FailMessage"] = $"Unable to import singers.";
					ModelState.AddModelError("", ex.Message);
                }
                catch (Exception ex)
                {
					TempData["FailMessage"] = $"Unable to import singers.";
					//return BadRequest(StaticMessages.UnknownError);
					ModelState.AddModelError("", ex.Message);
                }

            }
            else
            {
				//return BadRequest(StaticMessages.InvalidCSV);
				TempData["FailMessage"] = $"Unable to import singers.";
				ModelState.AddModelError("", "Please select a valid CSV file.");
            }
            return View(new CsvUploadVM<List<SingerCsvUpload>>
            {
                Data = new List<SingerCsvUpload>()
            });
        }

        [HttpGet, Route("Director")]
        public IActionResult Director()
        {
            return View();
        }


        [HttpPost, Route("Director")]
        public async Task<IActionResult> Director(IFormFile csvFile)
        {

            if (csvFile != null && csvFile.Length > 0)
            {
                //List<DirectorCsvUpload> newLocations = null;
                try
                {
                    var newDirectors = ReadDirectorCsv(csvFile);
                    Console.WriteLine(newDirectors);
                    var csvErrors = ValidateDirectors(newDirectors);
                    if (!csvErrors.All(s => s.IsValid))
                    {

                        //return Json(new
                        //{
                        //    csvErrors,
                        //});
                        MapModelStateErrors(csvErrors);
                        return View(new CsvUploadVM<List<DirectorCsvUpload>>
                        {
                            Data = new List<DirectorCsvUpload>()
                        });
                    }

                    var dbErrors = await AddDirectorsToDb(newDirectors);

                    MapModelStateErrors(csvErrors);
                    MapModelStateErrors(dbErrors.Errors);

					//return Json(new
					//{
					//    csvErrors,
					//    dbErrors
					//});
					TempData["SuccessMessage"] = $"Directors added.";
					return View(new CsvUploadVM<List<DirectorCsvUpload>>
                    {
                        Data = newDirectors,
                        SuccessCount = dbErrors.SuccessCount
                    });
                }
                catch (ApplicationException ex)
                { // invalid csv errors
                    Console.WriteLine(ex.Message);
					TempData["FailMessage"] = $"Unable to import directors.";
					ModelState.AddModelError("",ex.Message);
                    //return BadRequest(StaticMessages.UnknownError);
                }
                catch (Exception ex)
                {
					TempData["FailMessage"] = $"Unable to import directors.";
					ModelState.AddModelError("", StaticMessages.UnknownError);
                }


            }
            else
            {
				TempData["FailMessage"] = $"Unable to import directors.";
				ModelState.AddModelError("", StaticMessages.InvalidCSV);
            }
            return View(new CsvUploadVM<List<DirectorCsvUpload>>
            {
                Data = new List<DirectorCsvUpload>()
            });

        }


        [HttpGet, Route("Location")]
        public IActionResult Location()
        {
            return View();
        }

        [HttpPost, Route("Location")]
        public async Task<IActionResult> Location(IFormFile csvFile)
        {

            if (csvFile != null && csvFile.Length > 0)
            {
                try
                {
                    var newLocations = ReadLocationCsv(csvFile);

                    var csvErrors = ValidateLocations(newLocations);
                    if (!csvErrors.All(s => s.IsValid))
                    {
                        MapModelStateErrors(csvErrors);
                        return View(new CsvUploadVM<List<LocationCsvUpload>>
                        {
                            Data = new List<LocationCsvUpload>()
                        });
                        //return Json(new
                        //{
                        //    csvErrors
                        //});
                    }

                    var dbErrors = await AddLocationsToDb(newLocations);
                    MapModelStateErrors(csvErrors);
                    MapModelStateErrors(dbErrors.Errors);

					//return Json(new
					//{
					//    dbErrors,
					//
					//    csvErrors,
					//});
					TempData["SuccessMessage"] = $"Locations Added.";
					return View(new CsvUploadVM<List<LocationCsvUpload>>
                    {
                        Data = newLocations,
                        SuccessCount = dbErrors.SuccessCount
                    });
                }
                catch (ApplicationException ex)
                { // invalid csv errors
                    Console.WriteLine(ex.Message);
					TempData["FailMessage"] = $"Unable to import locations.";
					ModelState.AddModelError("", ex.Message);
                    //return Json(new
                    //{
                    //    ex.Message,
                    //});
                }
            }
            else
            {
				//return Json(new
				//{
				//    Message = "Please submit a valid CSV file"
				//});
				TempData["FailMessage"] = $"Unable to import locations.";
				ModelState.AddModelError("", "Please select a valid CSV file.");
            }
             return View(new CsvUploadVM<List<LocationCsvUpload>>
            {
                Data = new List<LocationCsvUpload>()
            });
        }

        private List<CsvValidationResponse> ValidateLocations(List<LocationCsvUpload> locations)
        {
            int lineCount = 1;
            var errors = new List<CsvValidationResponse>();
            //var failedToAdd = new List<string>();
            foreach (var location in locations)
            {
                var valid = location.IsValid();
                if (!valid.IsValid)
                {
                    valid.LineOfRecord = lineCount;
                    errors.Add(valid);

                }

                lineCount++;

            }


            return errors;
        }

        private async Task<DbErrorCollection> AddLocationsToDb(List<LocationCsvUpload> locations)
        {
            var collection = new DbErrorCollection();
            //int success = 0;
            foreach (var location in locations)
            {
                Console.WriteLine(location.IsAddingNewDirector());
                // check if the location exists 
                string newColor = DefaultColors[_context.Locations.Count() % DefaultColors.Count()];
                var locationObj = new Location
                {
                    City = location.City,
                    Color = newColor,
                };


                try
                {
                    _context.Add(locationObj);
                    await _context.SaveChangesAsync();

                    if (location.IsAddingNewDirector())
                    {
                        var dir = new Director {
                            FirstName = location.DirectorFirstName,
                            LastName = location.DirectorLastName,
                            PhoneNumber = location.DirectorPhone,
                            Email = location.DirectorEmail,
                        };
                        var result = await TryAddNewDirector(dir, locationObj);
                        if (!string.IsNullOrEmpty(result))
                        {
                            collection.Errors.Add(result);
                        }
                    }
                    else
                    {
                        var result = await TryLinkExistingDirector(location, locationObj);
                        if (!string.IsNullOrEmpty(result))
                        {
                            collection.Errors.Add(result);
                        }
                    }
                    collection.SuccessCount++;

                }
                catch (Exception e)
                {
                    if (e.GetBaseException().Message.Contains("UNIQUE"))
                    {
                        collection.Errors.Add($"There is already an existing location named " + locationObj.City);
                    }
                    else
                    {
                        collection.Errors.Add("There was an unexpected error when trying to add the location " + locationObj.City);
                    }
                    _context.Remove(locationObj);
                    //_context.Remove(lo);
                }

            }
            return collection;
        }

        private async Task<string> TryAddNewDirector(Director d, Location l)
        {
            try
            {
                _context.Add(d);
                await _context.SaveChangesAsync();
                _context.Add(new DirectorLocation { Location = l, Director = d });
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                if (e.Message.Contains("UNIQUE"))
                {
                    return "There is an existing director in the database, " +
                        "if you're trying to reference an existing director in bulk upload only include their first/last name.";
                }
                Console.WriteLine(e.Message);
                _context.Remove(d);
            }
            return "";
        }

        private async Task<string> TryLinkExistingDirector(LocationCsvUpload csvData, Location location)
        {
            var dir = _context.Directors
                            .Where(s => s.FirstName.ToLower() == csvData.DirectorFirstName.ToLower() && s.LastName.ToLower() == csvData.DirectorLastName.ToLower())
                            .FirstOrDefault();
            if (dir == null)
            {
                return $"There is no existing director with the name {csvData.DirectorFirstName} {csvData.DirectorLastName}. " +
                    $"If you're trying to create a new director please insert their full information (email/phone)";
            }
            else
            {
                _context.Add(new DirectorLocation { Location = location, Director = dir });
                await _context.SaveChangesAsync();
            }
            return "";
        }

        private List<CsvValidationResponse> ValidateDirectors(List<DirectorCsvUpload> directors)
        {

            int lineCount = 1;
            var errors = new List<CsvValidationResponse>();
            //var failedToAdd = new List<string>();
            foreach (var director in directors)
            {
                var valid = director.IsValid();
                if (!valid.IsValid)
                {
                    valid.LineOfRecord = lineCount;
                    errors.Add(valid);

                }

                lineCount++;

            }


            return errors;
        }


        private async Task<DbErrorCollection> AddDirectorsToDb(List<DirectorCsvUpload> directors)
        {
            var collection = new DbErrorCollection();
            //int success = 0;
            foreach (var director in directors)
            {
                // check if the location exists 
                var location = _context.Locations.Where(s => s.City.ToLower() == director.LocationName.ToLower()).FirstOrDefault();

                if (location == null)
                {
                    // continue we can't add a location without a location
                    // add a dbError
                    collection.Errors.Add("The location " + director.LocationName + " doesn't exist in our database. Did you misspell it?");
                    continue;
                }
                var directorObj = new Director
                {
                    FirstName = director.FirstName,
                    LastName = director.LastName,
                    PhoneNumber = director.PhoneNumber,
                    Email = director.Email,
                    IsActive = true
                };

                var lo = new DirectorLocation { Location = location, Director = directorObj };
                try
                {
                    _context.Add(directorObj);
                    _context.Add(lo);
                    await _context.SaveChangesAsync();
                    collection.SuccessCount++;

                }
                catch (Exception e)
                {
                    if (e.GetBaseException().Message.Contains("UNIQUE"))
                    {
                        collection.Errors.Add($"Unable to add the location {directorObj.FirstName} " +
                            $"{directorObj.LastName} as they either already exist in the database or the email/phone number " +
                            $"supplied is already associated with a location");
                    }
                    else
                    {
                        collection.Errors.Add("There was an unexpected error when trying to add the location " + director.FirstName + " " + director.LastName + ".");
                    }
                    _context.Remove(directorObj);
                    //_context.Remove(lo);
                }

            }
            return collection;
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
                    DOB = singer.DateOfBirth(),
                    EmergencyContactName=singer.ContactName
                };
                var lo = new SingerLocation { Location = location, Singer = singerObj };
                try
                {
                    _context.Add(singerObj);
                    _context.Add(lo);
                    await _context.SaveChangesAsync();
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

        private List<LocationCsvUpload> ReadLocationCsv(IFormFile csvFile)
        {
            if (csvFile != null && csvFile.Length > 0)
            {
                using (var stream = csvFile.OpenReadStream())
                {
                    var newDirectors = _csvService.ReadLocationCsvFile(stream).ToList();
                    return newDirectors;
                }
            }
            return [];
        }


        private void MapModelStateErrors(List<string> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error);
            }

        }

        private void MapModelStateErrors(List<CsvValidationResponse> errors)
        {
            foreach (var error in errors)
            {
                foreach(var e in error.Errors)
                {
                    ModelState.AddModelError("", e);
                }
            }

        }
    }
}
