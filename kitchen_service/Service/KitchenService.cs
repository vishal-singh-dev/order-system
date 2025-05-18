using kitchen_service.Contract;
using kitchen_service.Repository;
using Microsoft.EntityFrameworkCore;
using shared_library;
using System.Threading.Tasks;

namespace kitchen_service.Service
{
    public class KitchenService : IKitchenService
    {
        private readonly KitchenContext context;
        public KitchenService(KitchenContext _context)
        {
            context = _context;
        }
        public async Task<Response> updateOrderStatus(int Id, _ENUMs.orderStatus status)
        {
            try
            {
                var orderItem = await context.OrderItems.FirstOrDefaultAsync(x => x.orderItemId == Id);
                if (orderItem != null)
                {
                    orderItem.status = (int)status;
                    context.Entry(orderItem).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    return new Response() { code = System.Net.HttpStatusCode.OK, respnseMessage = "Order Status Updated" };

                }
                return new Response() { code = System.Net.HttpStatusCode.OK, respnseMessage = "failed to update status" };
            }
            catch (Exception)
            {
                return new Response() { code = System.Net.HttpStatusCode.BadRequest, respnseMessage = "There was an error" };
                throw;
            }
        }

       
    }
}
