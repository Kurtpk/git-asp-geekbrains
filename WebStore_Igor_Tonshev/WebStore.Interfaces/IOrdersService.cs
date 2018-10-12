using System.Collections.Generic;
using WebStore.DomainNew.Entities;
using WebStore.DomainNew.Models.Cart;
using WebStore.DomainNew.Models.Order;

namespace WebStore.Interfaces
{
    public interface IOrdersService
    {
        IEnumerable<Order> GetUserOrders(string userName);

        Order GetOrderById(int id);

        Order CreateOrder(OrderViewModel orderModel, CartViewModel transformCart, string userName);
    }
}
