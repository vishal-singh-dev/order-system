using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static shared_library._ENUMs;

namespace shared_library
{
    public  class orderCreation
    {
        public string OrderId{ get; set; }
        public List<itemDetail> items { get; set; } = new List<itemDetail>();
        public orderType orderType { get; set; } = orderType.newOrder;
    }
    public class itemDetail
    {
        public string Item { get; set; }
        public string instructions{ get; set; }
        public int quantity { get; set; }
        public int tableId{ get; set; }
        public int Status{ get; set; }
    }
}
