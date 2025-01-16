using Microsoft.EntityFrameworkCore;

namespace TV5_VolunteerEventMgmtApp.Data
{
    public class VolunteerEventMgmtAppInitializer
    {

        public static void Initialize(IServiceProvider serviceProvider, bool DeleteDatabase = false, bool UseMigrations = true, bool SeedSampleData = true) {

            using (var context = new VolunteerEventMgmtAppDbContext(serviceProvider.GetRequiredService<DbContextOptions<VolunteerEventMgmtAppDbContext>>()))
            {
                if(UseMigrations)
                {
                    context.Database.Migrate();
                }
            }
            

            
                

        }
    }
}
