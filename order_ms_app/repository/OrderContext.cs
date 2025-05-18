using Microsoft.EntityFrameworkCore;
using order_service.Models.dbModels;

namespace order_service.repository
{
    public class OrderContext:DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options):base(options)
        {

        }
        public DbSet<RestaurantOrders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
    }
}
