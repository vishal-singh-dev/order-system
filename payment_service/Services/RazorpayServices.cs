using Microsoft.Extensions.Options;
using payment_service.Contracts;
using payment_service.Models;
using Razorpay.Api;

namespace payment_service.Services
{
    public class RazorpayServices:IRazorpayServices
    {
        private readonly RazorpaySettings _razorpaySettings;
        private readonly RazorpayClient client;
        public RazorpayServices(IOptions<RazorpaySettings> razorpaySettings)
        {
            _razorpaySettings = razorpaySettings.Value;
            client = new RazorpayClient(_razorpaySettings.Key, _razorpaySettings.Secret);
        }
        public Dictionary<string, object> CreateOrder(decimal amount, string currency = "INR", string receipt = null)
        {
            try
            {
               
                Dictionary<string, object> options = new Dictionary<string, object>
                {
                    { "amount", amount * 100 },  
                    { "currency", currency },
                    { "receipt", receipt ?? $"receipt_{DateTime.Now.Ticks}" },
                    { "payment_capture", 1 }
                };

                Order order = client.Order.Create(options);
                return order.Attributes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating Razorpay order: {ex.Message}", ex);
            }
        }
        public bool VerifyPaymentSignature(string orderId, string paymentId, string signature)
        {
            try
            {
                string generatedSignature = Utils.Encrypt(
                    $"{orderId}|{paymentId}",
                    _razorpaySettings.Secret
                );

                return generatedSignature == signature;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error verifying payment signature: {ex.Message}", ex);
            }
        }
        public Dictionary<string, object> GetOrderDetails(string orderId)
        {
            try
            {
                Razorpay.Api.Order order = client.Order.Fetch(orderId);
                return order.Attributes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching order details: {ex.Message}", ex);
            }
        }
        public List<Dictionary<string, object>> GetPayments(string orderId = null)
        {
            try
            {
                Dictionary<string, object> options = new Dictionary<string, object>();

                if (!string.IsNullOrEmpty(orderId))
                {
                    options.Add("order_id", orderId);
                }

                List<Payment> payments = client.Payment.All(options);
                List<Dictionary<string, object>> paymentList = new List<Dictionary<string, object>>();

                foreach (Payment payment in payments)
                {
                    paymentList.Add(payment.Attributes);
                }

                return paymentList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching payments: {ex.Message}", ex);
            }
        }
        public Dictionary<string, object> RefundPayment(string paymentId, decimal amount = 0)
        {
            try
            {
                Payment payment = client.Payment.Fetch(paymentId);
                Dictionary<string, object> refundOptions = new Dictionary<string, object>();

                if (amount > 0)
                {
                    refundOptions.Add("amount", amount * 100);
                }

                Refund refund = payment.Refund(refundOptions);
                return refund.Attributes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing refund: {ex.Message}", ex);
            }
        }

    }
}   
