using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore_Igor_Tonshev.Infrastructure.Interfaces;
using WebStore_Igor_Tonshev.Models.Order;

namespace WebStore_Igor_Tonshev.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IOrdersService _ordersService;

        public ProfileController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Orders()
        {
            var orders = _ordersService.GetUserOrders(User.Identity.Name);

            var orderModels = new List<UserOrderViewModel>();

            foreach (var order in orders)
            {
                orderModels.Add(new UserOrderViewModel()
                {
                    Id = order.Id,
                    Name = order.Name,
                    Address = order.Address,
                    Phone = order.Phone,
                    TotalSum = order.OrderItems.Sum(o => o.Price * o.Quantity)
                });
            }

            return View(orderModels);
        }
    }
}