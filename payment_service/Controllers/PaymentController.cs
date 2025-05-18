using Microsoft.AspNetCore.Mvc;
using payment_service.Contracts;
using payment_service.Models.DTOs;

namespace payment_service.Controllers
{
    public class PaymentController: Controller
    {
        private readonly IRazorpayServices _razorpayService;
        public PaymentController(IRazorpayServices razorpayService)
        {
            _razorpayService = razorpayService;
        }
        
        [HttpPost("create-order")]
        public IActionResult creteaOrder([FromBody] OrderRequest request)
        {
            var orderResponse = _razorpayService.CreateOrder(
                     request.Amount,
                     request.Currency ?? "INR",
                     request.Receipt
                 );

            return Ok(orderResponse);
        }
        [HttpPost("verify-payment")]
        public IActionResult verifypayment([FromBody] PaymentVerificationRequest request)
        {
            bool isValid = _razorpayService.VerifyPaymentSignature(
                   request.OrderId,
                   request.PaymentId,
                   request.Signature
               );

            if (isValid)
            {
                // to add db operation to save payment details
                return Ok(new { Status = "Success", Message = "Payment verified successfully" });
            }
            else
            {
                return BadRequest(new { Status = "Failed", Message = "Payment signature verification failed" });
            }
        }
        [HttpGet("order/{orderId}")]
        public IActionResult GetOrderDetails(string orderId)
        {
            var orderDetails = _razorpayService.GetOrderDetails(orderId);
            return Ok(orderDetails);
        }
        [HttpGet("payments")]
        public IActionResult GetPayments([FromQuery] string orderId = null)
        {
            var payments = _razorpayService.GetPayments(orderId);
            return Ok(payments);
        }
        [HttpPost("refund")]
        public IActionResult RefundPayment([FromBody] RefundRequest request)
        {
            try
            {
                var refundResponse = _razorpayService.RefundPayment(request.PaymentId, request.Amount);
                return Ok(refundResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error processing refund", Error = ex.Message });
            }
        }
    }
}
