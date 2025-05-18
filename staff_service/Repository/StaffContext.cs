using Microsoft.EntityFrameworkCore;

namespace staff_service.Repository
{
    public class StaffContext:DbContext
    {
        public StaffContext(DbContextOptions<StaffContext> options) : base(options)
        {

        }

    }

}
