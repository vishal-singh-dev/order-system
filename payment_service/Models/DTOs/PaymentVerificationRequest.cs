namespace payment_service.Models.DTOs
{
    public class PaymentVerificationRequest
    {
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string Signature { get; set; }
    }
}
