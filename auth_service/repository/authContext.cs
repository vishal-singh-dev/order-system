using auth_service.Models;
using Microsoft.EntityFrameworkCore;

namespace auth_service.repository
{
    public class authContext:DbContext
    {
        public authContext(DbContextOptions<authContext> options) : base(options)
        {

        }
        public DbSet<User> users { get; set; }
        public DbSet<RefreshToken> tokens{ get; set; }
    }
}
