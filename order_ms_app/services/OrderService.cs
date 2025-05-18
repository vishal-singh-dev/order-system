using Microsoft.EntityFrameworkCore;
using order_service.Contracts;
using order_service.Models;
using order_service.Models.dbModels;
using order_service.Models.DTOs;
using order_service.repository;
using shared_library;
using System.Net;

namespace order_service.services
{
    public class OrderService : IOrderService
    {
        private readonly OrderContext _order;
        private readonly orderPublisher _publisher;
        public OrderService(OrderContext order, orderPublisher publisher)
        {
            _order = order;
            _publisher = publisher;
        }

        public async Task<Order> GetOrder(string OrderId)
        {
            var order = _order.Orders.FirstOrDefaultAsync(x => x.OrderId == OrderId);
            return new Order()
            {
                OrderId = order.Result.OrderId,
                TableId = order.Result.TableId,
                OrderTime = order.Result.OrderTime,
                TotalAmount = order.Result.totalAmount,
                PaymentStatus = order.Result.paymentStatus
            };
        }
        public async Task<List<OrderItemdetails>> GetItemsOrdered(string OrderId)
        {
            List<OrderItemdetails> items = new List<OrderItemdetails>();
            var _items = await _order.OrderItems.Where(x => x.orderId == OrderId).ToListAsync();
            _items.ForEach(x => items.Add(new OrderItemdetails() {
            orderItemId=x.orderItemId,
            orderId=x.orderId,
            itemId=x.itemId,
            price=x.itemPrice,
            quantity=x.quantity,
            
            }
            ));
            return items;
        }

        public async Task<List<Order>> GetOrders()
        {
            try
            {
                List<Order> _orders = new List<Order>();
                var orders = _order.Orders.ToList();
                orders?.ForEach(x => _orders.Add(new Order()
                {
                    OrderId = x.OrderId,
                    TableId = x.TableId,
                    OrderTime = x.OrderTime,
                    TotalAmount = x.totalAmount,
                    PaymentStatus = Convert.ToString(x.paymentStatus)
                }));
                return _orders;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Response> MakeOrder(List<OrderItemdetails> orderItems)
        {
            using (var tran = await _order.Database.BeginTransactionAsync())
            {
                try
                {
                    var order = orderItems.FirstOrDefault(); 
                    orderItems.ForEach(x =>
                    {

                        _order.OrderItems.Add(new OrderItems()
                        {
                            orderId = x.orderId,
                            itemId = x.itemId,
                            quantity = x.quantity,
                            specialInstructions = x.specialInstructions,
                            itemPrice = x.price,
                            status = (int)_ENUMs.orderStatus.Placed,

                        });
                    });
                    await _order.SaveChangesAsync();
                    await tran.CommitAsync();
                   
                    return new Response()
                    {
                        code = HttpStatusCode.OK,
                        respnseMessage = "Order Created"
                    };
                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                    return new Response()
                    {
                        code = HttpStatusCode.BadRequest,
                        respnseMessage = "Order Not Created"
                    };
                }
            }
        }

        public async Task<Response> UpdateOrder(OrderItemdetails item)
        {
            var Orderitem = _order.OrderItems.FirstOrDefaultAsync(x => x.orderItemId == item.orderItemId).Result;
            if (Orderitem != null)
            {
                if (!(Orderitem.status >= (int)_ENUMs.orderStatus.Prerparing))
                {
                    Orderitem.quantity = item.quantity;
                    Orderitem.specialInstructions = item.specialInstructions;
                    Orderitem.status = (int)_ENUMs.orderStatus.Updated;
                    _order.Entry(Orderitem).State = EntityState.Modified;
                    var result=_order.SaveChangesAsync().Result;
                    return new Response()
                    {
                        code = HttpStatusCode.OK,
                        respnseMessage = result > 0 ? "Order Updated" : "Order Not Updated"
                    };
                }
                return new Response()
                {
                    code = HttpStatusCode.OK,
                    respnseMessage = "Order is under process, cannot be updated"
                };
            }
            return new Response()
            {
                code = HttpStatusCode.OK,
                respnseMessage = "Order Not found"
            };
        }
        public async Task publishOrder(List<OrderItemdetails> orderItems)
        {
            
            var orderId=orderItems.FirstOrDefault().orderId;
            var createOrders = new orderCreation();
            createOrders.OrderId = orderId;
            orderItems.ForEach(x =>
            {
                createOrders.items.Add(new itemDetail()
                {
                    Item = x.itemDescription,
                    instructions = x.specialInstructions,
                    quantity = x.quantity,
                });



            });
            await _publisher.SubmitOrder(createOrders);
        }

        public async Task<Response> DeleteOrder(int itemId)
        {
            using (var tran= await _order.Database.BeginTransactionAsync())
            {
                try
                {
                    var Orderitem =await _order.OrderItems.FirstOrDefaultAsync(x => x.orderItemId == itemId);
                    if (Orderitem != null && Orderitem.status<=(int)_ENUMs.orderStatus.Updated)
                    {
                        _order.OrderItems.Remove(Orderitem);
                        var result = await _order.SaveChangesAsync();
                        await tran.CommitAsync();
                        return new Response()
                        {
                            code = HttpStatusCode.OK,
                            respnseMessage = result > 0 ? "Order Deleted" : "Order Not Deleted"
                        };
                    }
                    return new Response()
                    {
                        code = HttpStatusCode.BadRequest,
                        respnseMessage = "Order Not found"
                    };
                }
                catch(Exception)
                {
                    await tran.RollbackAsync();
                    return new Response()
                    {
                        code = HttpStatusCode.BadRequest,
                        respnseMessage =  "Order Not Deleted"
                    };
                }

            }
        }

       
    }
}
