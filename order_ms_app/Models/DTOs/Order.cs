namespace order_service.Models.DTOs
{
    public class Order
    {
        public string OrderId { get; set; }
        public int? TableId { get; set; }
        public DateTime OrderTime { get; set; } = DateTime.Now;
        public decimal? TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
    }
}
