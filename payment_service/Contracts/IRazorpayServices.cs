namespace payment_service.Contracts
{
 
    public interface IRazorpayServices
    {
        Dictionary<string, object> CreateOrder(decimal amount, string currency = "INR", string receipt = null);
        bool VerifyPaymentSignature(string orderId, string paymentId, string signature);
        Dictionary<string, object> GetOrderDetails(string orderId);
        List<Dictionary<string, object>> GetPayments(string orderId = null);
        Dictionary<string, object> RefundPayment(string paymentId, decimal amount = 0);
    }
}
