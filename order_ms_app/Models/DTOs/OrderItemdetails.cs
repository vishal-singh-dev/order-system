namespace order_service.Models.DTOs
{
    public class OrderItemdetails
    {
        public int orderItemId { get; set; }
        public string orderId { get; set; }
        public int itemId { get; set; }
        public string itemDescription { get; set; }
        public int tableId { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public string specialInstructions{ get; set; }
    }
}
