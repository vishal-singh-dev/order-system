using kitchen_service.Models.dbModels;
using Microsoft.EntityFrameworkCore;

namespace kitchen_service.Repository
{
    public class KitchenContext:DbContext
    {
        public KitchenContext(DbContextOptions<KitchenContext> options):base(options)
        {

        }
        public DbSet<OrderItems> OrderItems { get; set; }
    }
}
