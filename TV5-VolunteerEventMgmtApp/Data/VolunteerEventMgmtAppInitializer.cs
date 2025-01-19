using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TV5_VolunteerEventMgmtApp.Models;

namespace TV5_VolunteerEventMgmtApp.Data
{
	public class VolunteerEventMgmtAppInitializer
	{
		public static void Initialize(IServiceProvider serviceProvider,
			bool DeleteDatabase = false, bool UseMigrations = true, bool SeedSampleData = true)
		{
			using (var context = new VolunteerEventMgmtAppDbContext(
				serviceProvider.GetRequiredService<DbContextOptions<VolunteerEventMgmtAppDbContext>>()))
			{
				// Refresh the database as per the parameter options
				#region Prepare the Database
				try
				{
					// Note: .CanConnect() will return false if the database is not there!
					if (DeleteDatabase || !context.Database.CanConnect())
					{
						context.Database.EnsureDeleted(); // Delete the existing version 
						if (UseMigrations)
						{
							context.Database.Migrate(); // Create the Database and apply all migrations
						}
						else
						{
							context.Database.EnsureCreated(); // Create and update the database as per the Model
						}
						// Now create any additional database objects such as Triggers or Views
					}
					else // The database is already created
					{
						if (UseMigrations)
						{
							context.Database.Migrate(); // Apply all migrations
						}
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine("ERROR (1):");
					Debug.WriteLine(ex.GetBaseException().Message);
				}
				#endregion

				// Seed data needed for production and during development
				#region Seed Required Data
				try
				{
					// Add Locations
					if (!context.Locations.Any())
					{
						context.Locations.AddRange(
						 new Location
						 {
							 City = "St Catherines",
							 IsActive = true
						 }, new Location
						 {
							 City = "Hamilton",
							 IsActive = true
						 }, new Location
						 {
							 City = "Toronto",
							 IsActive = true
						 }, new Location
						 {
							 City = "Saskatoon",
							 IsActive = true
						 }, new Location
						 {
							 City = "Surrey",
							 IsActive = true
						 }, new Location
						 {
							 City = "Vancouver",
							 IsActive = true
						 });
						context.SaveChanges();
					}

					// Add Directors
					if (!context.Directors.Any())
					{
						context.Directors.AddRange(
						 new Director
						 {
							 FirstName = "John",
							 LastName = "Doe",
							 Email = "johnd88@gmail.com",
							 PhoneNumber = "9051234567"
						 }, new Director
						 {
							 FirstName = "Jane",
							 LastName = "Doe",
							 Email = "janed22@gmail.com",
							 PhoneNumber = "9051234522"
						 });
						context.SaveChanges();
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine("ERROR (2):");
					Debug.WriteLine(ex.GetBaseException().Message);
				}
				#endregion
			}
		}
	}
}