using Microsoft.EntityFrameworkCore;
using TV5_VolunteerEventMgmtApp.Models;
using System.Diagnostics;

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

                try
                {
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
                            VenueName = "",
                            Description = "",
                            Address = "",
                            ContactName = "",
                            ContactEmail = "",
                            ContactPhone = "",
                            LocationId = 1
                        },
                        new Venue
                        {
                            VenueName = "",
                            Description = "",
                            Address = "",
                            ContactName = "",
                            ContactEmail = "",
                            ContactPhone = "",
                            LocationId = 1
                        },
                        new Venue
                        {
                            VenueName = "",
                            Description = "",
                            Address = "",
                            ContactName = "",
                            ContactEmail = "",
                            ContactPhone = "",
                            LocationId = 1
                        },
                        new Venue
                        {
                            VenueName = "",
                            Description = "",
                            Address = "",
                            ContactName = "",
                            ContactEmail = "",
                            ContactPhone = "",
                            LocationId = 1
                        },
                        new Venue
                        {
                            VenueName = "",
                            Description = "",
                            Address = "",
                            ContactName = "",
                            ContactEmail = "",
                            ContactPhone = "",
                            LocationId = 1
                        },
                        new Venue
                        {
                            VenueName = "",
                            Description = "",
                            Address = "",
                            ContactName = "",
                            ContactEmail = "",
                            ContactPhone = "",
                            LocationId = 1
                        }
                            );
                        
                    }
                }
                catch
                {

                };


            }





        }
    }
}
