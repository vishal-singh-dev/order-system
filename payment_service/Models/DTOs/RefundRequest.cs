namespace payment_service.Models.DTOs
{
    public class RefundRequest
    {
        public string PaymentId { get; set; }
        public decimal Amount { get; set; }
    }
}
