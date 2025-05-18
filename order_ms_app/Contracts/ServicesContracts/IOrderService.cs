using order_service.Models.DTOs;
using shared_library;

namespace order_service.Contracts
{
    public interface IOrderService
    {
        Task<Response> MakeOrder(List<OrderItemdetails> orderItems);
        Task<Response> UpdateOrder(OrderItemdetails item);
        Task<Order> GetOrder(string OrderId);
        Task<List<Order>> GetOrders();
        Task publishOrder(List<OrderItemdetails> orderItems);
        Task<Response> DeleteOrder(int itemId);

    }
}
