﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces;
using SimpleMvcSitemap;
using WebStore.DomainNew.Filters;

namespace WebStore_Igor_Tonshev.Controllers
{
    public class SitemapController : Controller
    {
        private readonly IProductData _productData;

        public SitemapController(IProductData productData)
        {
            _productData = productData;
        }

        public IActionResult Index()
        {
            var nodes = new List<SitemapNode>
            {
                new SitemapNode(Url.Action("Index","Home")),
                new SitemapNode(Url.Action("Shop","Catalog")),
                new SitemapNode(Url.Action("BlogSingle","Home")),
                new SitemapNode(Url.Action("Blog","Home")),
                new SitemapNode(Url.Action("ContactUs","Home"))
            };

            var sections = _productData.GetSections();

            foreach (var section in sections)
            {
                nodes.Add(new SitemapNode(Url.Action("Shop", "Catalog", new { sectionId = section.Id })));
            }

            var brands = _productData.GetBrands();

            foreach (var brand in brands)
            {
                nodes.Add(new SitemapNode(Url.Action("Shop", "Catalog", new
                {
                    brandId = brand.Id
                })));
            }

            var products = _productData.GetProducts(new ProductFilter());

            foreach (var productDto in products.Products)
            {
                nodes.Add(new SitemapNode(Url.Action("ProductDetails",
                "Catalog", new { id = productDto.Id })));
            }

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}