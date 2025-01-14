using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace TV5_VolunteerEventMgmtApp.Data
{
    public class VolunteerEventMgmtAppDbContext : DbContext
    {
        public VolunteerEventMgmtAppDbContext(DbContextOptions<VolunteerEventMgmtAppDbContext> options)
            : base(options)
        {
        }
    }
}
