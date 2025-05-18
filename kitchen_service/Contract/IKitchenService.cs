using shared_library;
using static shared_library._ENUMs;

namespace kitchen_service.Contract
{
    public interface IKitchenService
    {
        Task<Response> updateOrderStatus(int Id,orderStatus status);
    }
}
