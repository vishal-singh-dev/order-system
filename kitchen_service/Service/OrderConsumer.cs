
using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using shared_library;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace kitchen_service.Service
{
    public class OrderConsumer:IConsumer<orderCreation>
    {
        public IHubContext<OrderHub> _context{ get; set; }
        public OrderConsumer(IHubContext<OrderHub> context)
        {
            _context = context;
        }
        public async Task Consume(ConsumeContext<orderCreation> context)
        {
            var stringMessage=  await ProcessMessageAsync(context.Message);
           await _context.Clients.All.SendAsync("OrderReceived",stringMessage);
        }
        private async Task<string> ProcessMessageAsync(orderCreation order)
        {
            try
            { 
                StringBuilder sb = new StringBuilder();
                order.items.ForEach(x => sb.Append($"order for {x.quantity} {x.Item}, instructions:{x.instructions??"no special instructions"}\n"));
                return sb.ToString();
            }
            catch (Exception ex)
            {

                return string.Empty;
            }
        }
        
    }
}
