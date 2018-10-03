﻿using Microsoft.AspNetCore.Mvc;
using WebStore_Igor_Tonshev.Infrastructure.Interfaces;

namespace WebStore_Igor_Tonshev.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;

        public CartViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public IViewComponentResult Invoke()
        {
            var cartView = _cartService.TransformCart();
            return View(cartView);
        }

    }
}
