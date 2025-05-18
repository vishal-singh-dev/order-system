using System.Net;

namespace order_service.Models.DTOs
{
    public class Response
    {
        public HttpStatusCode code{ get; set; }
        public string respnseMessage{ get; set; }
    }
}
