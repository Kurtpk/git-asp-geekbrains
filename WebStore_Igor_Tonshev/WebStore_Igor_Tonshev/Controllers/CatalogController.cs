using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.DomainNew.Filters;
using WebStore.Interfaces;
using WebStore.DomainNew.Models.Product;
using Microsoft.Extensions.Configuration;
using WebStore.DomainNew.Models;

namespace WebStore_Igor_Tonshev.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;
        private readonly IConfiguration _configuration;

        public CatalogController(IProductData productData, IConfiguration configuration)
        {
            _productData = productData;
            _configuration = configuration;
        }

        public IActionResult Shop(int? sectionId, int? brandId, int page = 1)
        {
            var totalCount = GetTotalCount(sectionId, brandId);

            var model = new CatalogViewModel()
            {
                BrandId = brandId,
                SectionId = sectionId,
                PageViewModel = new PageViewModel
                {
                    PageSize = int.Parse(_configuration["PageSize"]),
                    PageNumber = page,
                    TotalItems = totalCount
                }
            };

            return View(model);
        }

        private int GetTotalCount(int? sectionId, int? brandId)
        {
            var products = _productData.GetProducts(new ProductFilter
            {
                BrandId = brandId,
                SectionId = sectionId,
                Page = 1,
                PageSize = int.Parse(_configuration["PageSize"])
            });
            return products.TotalCount;
        }

        public IActionResult GetFilteredItems(int? sectionId, int? brandId, int page = 1)
        {
            var productsModel = GetProducts(sectionId, brandId, page);

            return PartialView("Partial/_ProductItems", productsModel);
        }

        private IEnumerable<ProductViewModel> GetProducts(int? sectionId, int? brandId, int page)
        {
            var products = _productData.GetProducts(new ProductFilter
            {
                BrandId = brandId,
                SectionId = sectionId,
                Page = page,
                PageSize = int.Parse(_configuration["PageSize"])
            });

            return products.Products.Select(p => new ProductViewModel()
            {
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                Name = p.Name,
                Order = p.Order,
                Price = p.Price,
                Brand = p.Brand != null ? p.Brand.Name : string.Empty
            }).ToList();
        }

        public IActionResult ProductDetails(int id)
        {
            var product = _productData.GetProductById(id);
            if (product == null)
                return NotFound();

            return View(new ProductViewModel
            {
                Id = product.Id,
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                Order = product.Order,
                Price = product.Price,
                Brand = product.Brand != null ? product.Brand.Name : string.Empty
            });
        }
    }
}