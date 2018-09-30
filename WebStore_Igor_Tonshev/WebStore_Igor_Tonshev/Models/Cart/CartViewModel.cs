using System.Collections.Generic;
using System.Linq;
using WebStore_Igor_Tonshev.Models.Product;

namespace WebStore_Igor_Tonshev.Models.Cart
{
    public class CartViewModel
    {
        public Dictionary<ProductViewModel, int> Items { get; set; }

        public int ItemsCount
        {
            get
            {
                return Items?.Sum(x => x.Value) ?? 0;
            }
        }

        public decimal TotalPrice
        {
            get
            {
                return Items?.Sum(x => x.Value * x.Key.Price) ?? 0;
            }
        }
    }
}
