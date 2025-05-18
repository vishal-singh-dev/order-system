using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace order_service.Models.dbModels
{
    [Table("RestaurantOrders")]
    [PrimaryKey("OrderId")]
    public class RestaurantOrders
    {
        public string OrderId { get; set; }
        public int TableId { get; set; }
        public DateTime OrderTime { get; set; } = DateTime.Now;
        public decimal? totalAmount { get; set; }
        public string? paymentStatus { get; set; }

    }
}
