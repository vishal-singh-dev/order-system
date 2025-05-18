using kitchen_service.Repository;
using kitchen_service.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static shared_library._ENUMs;

namespace kitchen_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KitchenController : Controller
    {
        private readonly KitchenService _service;
        public KitchenController(KitchenService service)
        {
            _service = service; 
        }
        [HttpGet]
        public async Task<ActionResult> UpdateOrderStatus(int Id, orderStatus status)
        {
            var data = await _service.updateOrderStatus(Id, status);
            return Ok(data);
        }
    }
}
