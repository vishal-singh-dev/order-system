using Microsoft.AspNetCore.Mvc;
using order_service.Contracts;
using order_service.Models.DTOs;
using System.Threading.Tasks;

namespace order_service.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrderController : Controller
    {
        readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            this._orderService= orderService;
        }
        [HttpGet]
        [Route("get-orders")]
        public async Task<ActionResult> GetOrders()
        {
            var data= await _orderService.GetOrders();
            return Ok(data);
        }
        [HttpGet]
        [Route("get-orders/{id}")]
        public async Task<ActionResult> GetOrder(string id)
        {
            var data=await _orderService.GetOrder(id);
            return Ok(data);
        }
        [HttpPost]
        [Route("make-order")]
        public async Task<ActionResult> MakeOrder(List<OrderItemdetails> orderItems)
        {
            var data=await _orderService.MakeOrder(orderItems);
            await _orderService.publishOrder(orderItems);
            return Ok(data);
        }
        [HttpPost]
        [Route("update-order")]
        public ActionResult MakeOrder(OrderItemdetails order)
        {
           var data = _orderService.UpdateOrder(order);
            return Ok(data);
        }
        [HttpDelete]
        [Route("delete-order")]
        public ActionResult DeleteOrder(int itemId)
        {
            var data = _orderService.DeleteOrder(itemId);
            return Ok(data);
        }

    }
}
