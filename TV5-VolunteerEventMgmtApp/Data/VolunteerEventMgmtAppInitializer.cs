using Microsoft.EntityFrameworkCore;
using TV5_VolunteerEventMgmtApp.Models;
using System.Diagnostics;
using System;

namespace TV5_VolunteerEventMgmtApp.Data
{
    public class VolunteerEventMgmtAppInitializer
    {

        public static void Initialize(IServiceProvider serviceProvider, bool DeleteDatabase = false, bool UseMigrations = true, bool SeedSampleData = true) {

            using (var context = new VolunteerEventMgmtAppDbContext(serviceProvider.GetRequiredService<DbContextOptions<VolunteerEventMgmtAppDbContext>>()))
            {
                try
                {
                    if(DeleteDatabase || !context.Database.CanConnect())
                    {
                        context.Database.EnsureDeleted();
                        if (UseMigrations)
                        {
                            context.Database.Migrate();
                        }
                        else
                        {
                            context.Database.EnsureCreated();
                        }
                    }
                    else
                    {
                        if(UseMigrations)
                        {
                            context.Database.Migrate();
                        }
                    }

                    
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.GetBaseException().Message);
                }


                if (SeedSampleData)
                {
                    try
                    {
                        Random random = new Random();
                        if (!context.Locations.Any())
                        {
                            context.Locations.AddRange(
                                new Location
                                {
                                    City = "Niagara",
                                    IsActive = true
                                },
                                new Location
                                {
                                    City = "Hamilton",
                                    IsActive = true
                                },
                                new Location
                                {
                                    City = "Toronto",
                                    IsActive = true
                                },
                                new Location
                                {
                                    City = "Saskatoon",
                                    IsActive = true
                                },
                                new Location
                                {
                                    City = "Surrey",
                                    IsActive = true
                                },
                                new Location
                                {
                                    City = "Vancouver",
                                    IsActive = false
                                }
                                );
                            context.SaveChanges();
                        }

                        if (!context.Directors.Any())
                        {
                            context.Directors.AddRange(
                                new Director
                                {
                                    FirstName = "John",
                                    LastName = "Smith",
                                    PhoneNumber = "4162347654",
                                    Email = "torontoDir@email.com"
                                },
                                new Director
                                {
                                    FirstName = "Sally",
                                    LastName = "Andrews",
                                    PhoneNumber = "8906542765",
                                    Email = "seeded@email.com"
                                },
                                new Director
                                {
                                    FirstName = "Molly",
                                    LastName = "Maxine",
                                    PhoneNumber = "7682763876",
                                    Email = "MMaxine@email.com"
                                },
                                new Director
                                {
                                    FirstName = "Jason",
                                    LastName = "Reeter",
                                    PhoneNumber = "5467892098",
                                    Email = "JReeter@email.com"
                                },
                                new Director
                                {
                                    FirstName = "Sergio",
                                    LastName = "Super",
                                    PhoneNumber = "9872437648",
                                    Email = "SSuper@email.com"
                                },
                                new Director
                                {
                                    FirstName = "Hong",
                                    LastName = "Dian",
                                    PhoneNumber = "5472986789",
                                    Email = "HDian@email.com"
                                },
                                new Director
                                {
                                    FirstName = "Clay",
                                    LastName = "Brittle",
                                    PhoneNumber = "8562530987",
                                    Email = "CBrittle@email.com"
                                }
                                );
                            context.SaveChanges();
                        }

                        if (!context.Venues.Any())
                        {
                            context.Venues.AddRange(
                                new Venue
                                {
                                    VenueName = "Building 1",
                                    Description = "its a building that can host 75 people",
                                    Address = "123 tasker street",
                                    ContactName = "Barack Obama",
                                    ContactEmail = "building1@email.com",
                                    ContactPhone = "7862653765",
                                    LocationId = 1
                                },
                            new Venue
                            {
                                VenueName = "Building 2",
                                Description = "This building is used for choir and Volunteer Events",
                                Address = "165 Front Street",
                                ContactName = "James Target",
                                ContactEmail = "building2@email.com",
                                ContactPhone = "8764352098",
                                LocationId = 1
                            },
                            new Venue
                            {
                                VenueName = "Building 3",
                                Description = "This venue hosts up to 60 people",
                                Address = "7654 Younge street",
                                ContactName = "Jennifer Cooke",
                                ContactEmail = "Building3@email.com",
                                ContactPhone = "6547891234",
                                LocationId = 2
                            },
                            new Venue
                            {
                                VenueName = "Building 4",
                                Description = "this venue hosts up to 80 people",
                                Address = "876 Queen Street",
                                ContactName = "Laura Bates",
                                ContactEmail = "building4@email.com",
                                ContactPhone = "7652349876",
                                LocationId = 3
                            },
                            new Venue
                            {
                                VenueName = "Building 5",
                                Description = "this venue hosts up to 100 people",
                                Address = "543 King Street",
                                ContactName = "Haily Weisse",
                                ContactEmail = "building5@email.com",
                                ContactPhone = "7652341098",
                                LocationId = 4
                            },
                            new Venue
                            {
                                VenueName = "Building 6",
                                Description = "this venue can host up to 30 people",
                                Address = "546 West Street",
                                ContactName = "Stefan Trails",
                                ContactEmail = "building6@email.com",
                                ContactPhone = "9876665432",
                                LocationId = 5
                            },
                            new Venue
                            {
                                VenueName = "Building 7",
                                Description = "this venue can host up to 300 people",
                                Address = "987 Main Street",
                                ContactName = "Mona Lanta",
                                ContactEmail = "building7@email.com",
                                ContactPhone = "6785431234",
                                LocationId = 6
                            },
                            new Venue
                            {
                                VenueName = "Building 8",
                                Description = "this venue can host up to 75 people",
                                Address = "777 Duff Street",
                                ContactName = "Jim Monts",
                                ContactEmail = "building8@email.com",
                                ContactPhone = "9998764567",
                                LocationId = 4
                            },
                            new Venue
                            {
                                VenueName = "Building 9",
                                Description = "this venue can host up to 25 people",
                                Address = "765 Lake Street",
                                ContactName = "Troy Turner",
                                ContactEmail = "building9@email.com",
                                ContactPhone = "8785431876",
                                LocationId = 2
                            },
                            new Venue
                            {
                                VenueName = "building10",
                                Description = "this venue can host up to 100 people",
                                Address = "666 Water Street",
                                ContactName = "Kevin Shore",
                                ContactEmail = "building10@email.com",
                                ContactPhone = "7697775432",
                                LocationId = 4
                            }
                                );
                            context.SaveChanges();

                        }

                        if (!context.Singers.Any())
                        {
                            string[] firstNames = new string[] { "Woodstock", "Violet", "Charlie", "Lucy", "Linus", "Franklin", "Marcie", "Schroeder", "Lyric", "Antoinette", "Kendal", "Vivian", "Ruth", "Jamison", "Emilia", "Natalee", "Yadiel", "Jakayla", "Lukas", "Moses", "Kyler", "Karla", "Chanel", "Tyler", "Camilla", "Quintin", "Braden", "Clarence" };
                            string[] lastNames = new string[] { "Hightower", "Broomspun", "Jones", "Bloggs" };

                            //Choose a random HashSet of 5 first names
                            List<string> selectedFirstNames = new List<string>();
                                while (selectedFirstNames.Count() < 100)
                                {
                                    selectedFirstNames.Add(firstNames[random.Next(firstNames.Length)]);
                                }

                                foreach (string firstName in selectedFirstNames)
                                {
                                    //Construct some Singer details
                                    
                                    Singer singer = new Singer()
                                    {
                                        FirstName = firstName,
                                        LastName = lastNames[random.Next(0,lastNames.Length)],
                                        Email = (firstName + random.Next(11, 111).ToString() + "@outlook.com").ToLower(),
                                        DOB = DateOnly.FromDateTime(DateTime.Today.AddDays(-random.Next(2922, 6575))),
                                        Phone = random.Next(2, 10).ToString() + random.Next(213214131, 989898989).ToString(),
                                        isActive = true,
                                        LocationId = random.Next(1, 7)
                                    };
                                    try
                                    {
                                        //Could be a duplicate Email
                                        context.Singers.Add(singer);
                                        context.SaveChanges();
                                    }
                                    catch (Exception)
                                    {
                                        //so skip it and go on to the next
                                        context.Singers.Remove(singer);
                                    }
                                }
                            

                        }

                        if (!context.DirectorLocations.Any())
                        {
                            int[] locationIds = context.Locations.Select(d => d.ID).ToArray();
                            int directorIndex = 0;
                            int[] DirectorIDS = context.Directors.Select(d => d.ID).ToArray();
                            foreach (var location in locationIds)
                            {
                                int locationDirector = DirectorIDS[directorIndex];
                                directorIndex++;
                                DirectorLocation directorLocation = new DirectorLocation()
                                {
                                    DirectorID = locationDirector,
                                    LocationID = location
                                };
                                try
                                {
                                    context.DirectorLocations.Add(directorLocation);
                                    context.SaveChanges();
                                }
                                catch
                                {
                                    context.DirectorLocations.Remove(directorLocation);
                                }
                            }
                        }


                        if (!context.AttendeesSheets.Any())
                        {
                           
                            
                            int[] locationIds = context.Locations.Select(d => d.ID).ToArray();
                            foreach (var location in locationIds)
                            {
                                int[] VenueIDs = context.Venues.Where(d => d.LocationId == location).Select(d => d.ID).ToArray();
                                int directorID = context.DirectorLocations.Where(d => d.LocationID == location).Select(d => d.DirectorID).FirstOrDefault();

                                for(int i = 0; i < 10; i++)
                                {
                                    

                                    DateTime randDay = DateTime.Now.AddDays(-random.Next(1,90));
                                    
                                   
                                    AttendanceSheet sheet = new AttendanceSheet
                                    {
                                        DirectorId = directorID,
                                        Notes = "example notes.",
                                        LocationId = location,
                                        
                                        StartTime = randDay.AddHours(random.Next(10, 12)),
                                        EndTime = randDay.AddHours(random.Next(13, 16))
                                    };
                                    try
                                    {
                                        context.AttendeesSheets.Add(sheet);
                                        context.SaveChanges();
                                    }
                                    catch(Exception ex)
                                    {
                                        context.AttendeesSheets.Remove(sheet);
                                    }
                                }



                            }
                        }

                        if(!context.SingerLocations.Any())
                        {
                            int singerIndex = 0;
                            int[] singerIDs = context.Singers.Select(d => d.Id).ToArray();
                            int[] locationIds = context.Locations.Select(d => d.ID).ToArray();

                            foreach(var location in locationIds)
                            {
                                for(int j =0; j < context.Singers.Count()/context.Locations.Count() ; j++)
                                {
                                    SingerLocation singerLocation = new SingerLocation()
                                    {
                                        SingerId = singerIDs[singerIndex],
                                        LocationId = location
                                    };
                                    singerIndex++;
                                    try
                                    {
                                        context.SingerLocations.Add(singerLocation);
                                        context.SaveChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        context.SingerLocations.Remove(singerLocation);
                                    }
                                }
                            }
                            
                        }


                        if(!context.Attendees.Any())
                        {
                            int[] locationIds = context.Locations.Select(d => d.ID).ToArray();
                            foreach (var location in locationIds)
                            {
                                AttendanceSheet[] sheets = context.AttendeesSheets.Where(d => d.LocationId == location).ToArray();
                                SingerLocation[] singers = context.SingerLocations.Where(d => d.LocationId == location).ToArray();

                                foreach(var sheet in sheets)
                                {
                                    int howMany = random.Next(2, 10);
                                    for (int j = 1; j < howMany; j++)
                                    {
                                        Attendee attendee = new Attendee()
                                        {
                                            AttendenceSheetId = sheet.Id,
                                            SingerId = singers[random.Next(1, singers.Length)].SingerId
                                        };
                                        try
                                        {
                                            context.Attendees.Add(attendee);
                                            context.SaveChanges();
                                        }
                                        catch(Exception ex)
                                        {
                                            context.Attendees.Remove(attendee);
                                        }
                                    }
                                }
                            }
                        }


                    }
                    catch (Exception ex)
                    {

                    };
                }


            }





        }
    }
}
