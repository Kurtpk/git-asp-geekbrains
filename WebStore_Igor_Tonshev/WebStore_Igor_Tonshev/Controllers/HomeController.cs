using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebStore_Igor_Tonshev.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }
        public IActionResult BlogSingle()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult NotFoundPage()
        {
            return View();
        }
    }
}