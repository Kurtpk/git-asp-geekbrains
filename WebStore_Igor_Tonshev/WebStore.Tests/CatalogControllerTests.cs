using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.DomainNew.Dto.Product;
using WebStore.DomainNew.Filters;
using WebStore.DomainNew.Models.Product;
using WebStore.Interfaces;
using WebStore_Igor_Tonshev.Controllers;

namespace WebStore.Tests
{
    [TestClass]
    public class CatalogControllerTests
    {
        [TestMethod]
        public void ProductDetails_Returns_View_With_Correct_Item()
        {
            // Arrange
            var productMock = new Mock<IProductData>();
            var configurationMock = new Mock<IConfiguration>();

            configurationMock.SetupGet(x => x[It.IsAny<string>()]).Returns("3");

            productMock.Setup(p => p.GetProductById(It.IsAny<int>())).Returns(new ProductDto()
            {
                Id = 1,
                Name = "Test",
                ImageUrl = "TestImage.jpg",
                Order = 0,
                Price = 10,
                Brand = new BrandDto()
                {
                    Id = 1,
                    Name = "TestBrand"
                }
            });

            var controller = new CatalogController(productMock.Object, configurationMock.Object);

            // Act
            var result = controller.ProductDetails(1);

            // Assert
            var viewResult = Xunit.Assert.IsType<ViewResult>(result);
            var model = Xunit.Assert.IsAssignableFrom<ProductViewModel>(
                viewResult.ViewData.Model);
            Xunit.Assert.Equal(1, model.Id);
            Xunit.Assert.Equal("Test", model.Name);
            Xunit.Assert.Equal(10, model.Price);
            Xunit.Assert.Equal("TestBrand", model.Brand);
        }

        [TestMethod]
        public void ProductDetails_Returns_NotFound()
        {
            // Arrange
            var productMock = new Mock<IProductData>();
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.SetupGet(x => x[It.IsAny<string>()]).Returns("3");
            productMock.Setup(p => p.GetProductById(It.IsAny<int>())).Returns((ProductDto)null);
            var controller = new CatalogController(productMock.Object, configurationMock.Object);

            // Act
            var result = controller.ProductDetails(1);

            // Assert
            Xunit.Assert.IsType<NotFoundResult>(result);
        }

        [TestMethod]
        public void Shop_Method_Returns_Correct_View()
        {
            // Arrange
            var productMock = new Mock<IProductData>();
            var configurationMock = new Mock<IConfiguration>();

            configurationMock.SetupGet(x => x[It.IsAny<string>()]).Returns("3");

            var products = new PagedProductDto();
            products.Products = new List<ProductDto>
            {
                new ProductDto()
                {
                    Id = 1,
                    Name = "Test",
                    ImageUrl = "TestImage.jpg",
                    Order = 0,
                    Price = 10,
                    Brand = new BrandDto()
                    {
                        Id = 1,
                        Name = "TestBrand"
                    }
                },
                new ProductDto()
                {
                    Id = 2,
                    Name = "Test2",
                    ImageUrl = "TestImage2.jpg",
                    Order = 1,
                    Price = 22,
                    Brand = new BrandDto()
                    {
                        Id = 1,
                        Name = "TestBrand"
                    }
                }
            };

            productMock.Setup(p => p.GetProducts(It.IsAny<ProductFilter>())).Returns(products);

            var controller = new CatalogController(productMock.Object, configurationMock.Object);

            // Act
            var result = controller.Shop(1, 5);

            // Assert
            var viewResult = Xunit.Assert.IsType<ViewResult>(result);
            var model = Xunit.Assert.IsAssignableFrom<CatalogViewModel>(
                viewResult.ViewData.Model);

            Xunit.Assert.Equal(5, model.BrandId);
            Xunit.Assert.Equal(1, model.SectionId);
        }
    }
}
