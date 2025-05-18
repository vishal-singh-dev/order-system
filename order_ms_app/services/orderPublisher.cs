using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using order_service.Models.DTOs;
using shared_library;
using Microsoft.Extensions.Options;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Response = shared_library.Response;

namespace order_service.services
{
    public class orderPublisher
    {

        private readonly IPublishEndpoint _publish;
        public orderPublisher(IPublishEndpoint publish)
        {
            _publish = publish;
        }
        public async Task<Response> SubmitOrder(orderCreation order)
        {
            await _publish.Publish(order);
            return new Response()
            {
                code = System.Net.HttpStatusCode.OK,
                respnseMessage = "Order submitted successfully"
            }
            ;
        }
    }
}
