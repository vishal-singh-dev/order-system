using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace kitchen_service.Models.dbModels
{
    [Table("OrderItems")]
    [PrimaryKey("orderItemId")]
    public class OrderItems
    {
        public int orderItemId { get; set; }
        public string orderId { get; set; }
        public int itemId { get; set; }
        public int quantity { get; set; }
        public string specialInstructions { get; set; }
        public decimal itemPrice{ get; set; }
        public int status { get; set; }
    }
}
