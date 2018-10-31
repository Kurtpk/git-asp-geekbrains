using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces;
using WebStore.DomainNew.Models.Cart;
using WebStore.DomainNew.Models.Order;
using WebStore.DomainNew.Dto.Order;

namespace WebStore_Igor_Tonshev.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrdersService _ordersService;

        public CartController(ICartService cartService, IOrdersService ordersService)
        {
            _cartService = cartService;
            _ordersService = ordersService;
        }

        public IActionResult Details()
        {
            var model = new DetailsViewModel()
            {
                CartViewModel = _cartService.TransformCart(),
                OrderViewModel = new OrderViewModel()
            };
            return View(model);
        }

        public IActionResult DecrementFromCart(int id)
        {
            _cartService.DecrementFromCart(id);
            return Json(new { id, message = "Количество товара уменьшено на 1" });
        }

        public IActionResult RemoveFromCart(int id)
        {
            _cartService.RemoveFromCart(id);
            return Json(new { id, message = "Товар удален из корзины" });
        }

        public IActionResult RemoveAll()
        {
            _cartService.RemoveAll();
            return RedirectToAction("Details");
        }

        public IActionResult AddToCart(int id)
        {
            _cartService.AddToCart(id);
            return Json(new { id, message = "Товар добавлен в корзину" });
        }

        public IActionResult GetCartView()
        {
            return ViewComponent("Cart");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CheckOut(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var orderModel = new CreateOrderModel()
                {
                    OrderItems = _cartService.TCart(),
                    OrderViewModel = model
                };

                var orderResult = _ordersService.CreateOrder(orderModel, User.Identity.Name);
                _cartService.RemoveAll();
                return RedirectToAction("OrderConfirmed", new { id = orderResult.Id });
            }

            var detailsModel = new DetailsViewModel()
            {
                CartViewModel = _cartService.TransformCart(),
                OrderViewModel = model
            };
            return View("Details", detailsModel);
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}