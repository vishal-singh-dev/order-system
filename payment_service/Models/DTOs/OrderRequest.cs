namespace payment_service.Models.DTOs
{
    public class OrderRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Receipt { get; set; }
    }
}
