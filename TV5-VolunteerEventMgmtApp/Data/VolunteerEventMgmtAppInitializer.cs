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

                    if(!context.AttendeesSheets)
                }
                catch
                {

                };


            }





        }
    }
}
