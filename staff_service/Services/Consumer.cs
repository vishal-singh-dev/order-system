using MassTransit;
using Microsoft.AspNetCore.SignalR;
using shared_library;

namespace staff_service.Services
{
    public class Consumer:IConsumer<itemDetail>
    {
        private readonly OrderHub _context;
        public Consumer(OrderHub context)
        {
            _context = context;
        }
        public async Task Consume(ConsumeContext<itemDetail> context)
        {
            var stringMessage = $"Order for {context.Message.Item} for table no {context.Message.tableId} is : {context.Message.Status}";
            await _context.Clients.All.SendAsync("OrderUpdates",stringMessage);
        }
    }
}
