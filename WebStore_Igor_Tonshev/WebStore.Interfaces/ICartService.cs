using System.Collections.Generic;
using WebStore.DomainNew.Dto.Order;
using WebStore.DomainNew.Models.Cart;

namespace WebStore.Interfaces
{
    public interface ICartService
    {
        void DecrementFromCart(int id);

        void RemoveFromCart(int id);

        void RemoveAll();

        void AddToCart(int id);

        CartViewModel TransformCart();

        List<OrderItemDto> TCart();
    }
}
