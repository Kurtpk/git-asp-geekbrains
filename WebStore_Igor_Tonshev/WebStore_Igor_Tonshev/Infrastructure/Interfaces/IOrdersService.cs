using System.Collections.Generic;
using WebStore.DomainNew.Entities;
using WebStore_Igor_Tonshev.Models.Cart;
using WebStore_Igor_Tonshev.Models.Order;

namespace WebStore_Igor_Tonshev.Infrastructure.Interfaces
{
    public interface IOrdersService
    {
        IEnumerable<Order> GetUserOrders(string userName);

        Order GetOrderById(int id);

        Order CreateOrder(OrderViewModel orderModel, CartViewModel transformCart, string userName);
    }
}
