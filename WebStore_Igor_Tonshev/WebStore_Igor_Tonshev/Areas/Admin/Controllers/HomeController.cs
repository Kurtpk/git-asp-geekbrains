using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebStore.DomainNew.Dto.Product;
using WebStore.DomainNew.Filters;
using WebStore.DomainNew.Models.Product;
using WebStore.Interfaces;

namespace WebStore_Igor_Tonshev.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Administrator")]
    public class HomeController : Controller
    {
        private readonly IProductData _productData;

        public HomeController(IProductData productData)
        {
            _productData = productData;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductList()
        {
            var products = _productData.GetProducts(new ProductFilter());
            return View(products);
        }

        public IActionResult Edit(int? id)
        {
            var notParentSections = _productData.GetSections().Where(s => s.ParentId != null);
            var brands = _productData.GetBrands();

            if (!id.HasValue)
            {
                return View(new ProductViewModel()
                {
                    Sections = new SelectList(notParentSections, "Id", "Name"),
                    Brands = new SelectList(brands, "Id", "Name")
                });
            }

            var product = _productData.GetProductById(id.Value);
            if (product == null)
                return NotFound();

            return View(new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Section = product.Section.Name,
                SectionId = product.Section.Id,
                Brand = product.Brand?.Name,
                BrandId = product.Brand?.Id,
                Brands = new SelectList(brands, "Id", "Name", product.Brand?.Id),
                Sections = new SelectList(notParentSections, "Id", "Name", product.Section.Id)
            });
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel model)
        {
            var notParentSections = _productData.GetSections().Where(s => s.ParentId != null);
            var brands = _productData.GetBrands();
            if (ModelState.IsValid)
            {
                var productDto = new ProductDto()
                {
                    Id = model.Id,
                    ImageUrl = model.ImageUrl,
                    Name = model.Name,
                    Order = model.Order,
                    Price = model.Price,
                    Brand = model.BrandId.HasValue
                        ? new BrandDto()
                        {
                            Id = model.BrandId.Value
                        }
                        : null,
                    Section = new SectionDto()
                    {
                        Id = model.SectionId
                    }
                };
                if (model.Id > 0)
                {
                    _productData.UpdateProduct(productDto);
                }
                else
                {
                    _productData.CreateProduct(productDto);
                }
                return RedirectToAction(nameof(ProductList));
            }

            model.Brands = new SelectList(brands, "Id", "Name", model.BrandId);
            model.Sections = new SelectList(notParentSections, "Id", "Name", model.SectionId);

            return View(model);
        }

        //[Route("product/delete/{id}")]
        //[HttpPut]
        public IActionResult Delete(int id)
        {
            _productData.DeleteProduct(id);
            return RedirectToAction(nameof(ProductList));
        }
    }
}