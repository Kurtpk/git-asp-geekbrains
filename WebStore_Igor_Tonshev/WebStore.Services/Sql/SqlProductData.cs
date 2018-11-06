using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.DomainNew.Dto;
using WebStore.DomainNew.Dto.Product;
using WebStore.DomainNew.Entities;
using WebStore.DomainNew.Filters;
using WebStore.Interfaces;

namespace WebStore.Services.Sql
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreContext _context;

        public SqlProductData(WebStoreContext context)
        {
            _context = context;
        }

        public IEnumerable<SectionDto> GetSections()
        {
            return _context.Sections.Select(s => new SectionDto()
            {
                Id = s.Id,
                Name = s.Name,
                Order = s.Order,
                ParentId = s.ParentId
            }).ToList();
        }

        public SectionDto GetSectionById(int id)
        {
            var section = _context.Sections.FirstOrDefault(c => c.Id == id);

            if (section != null)
            {
                return new SectionDto()
                {
                    Id = section.Id,
                    Name = section.Name,
                    ParentId = section.ParentId,
                    Order = section.Order
                };
            }

            return null;
        }

        public IEnumerable<BrandDto> GetBrands()
        {
            return _context.Brands.Select(b => new BrandDto()
            {
                Id = b.Id,
                Name = b.Name,
                Order = b.Order
            }).ToList();
        }

        public BrandDto GetBrandById(int id)
        {
            var brand = _context.Brands.FirstOrDefault(c => c.Id == id);

            if (brand != null)
            {
                return new BrandDto()
                {
                    Id = brand.Id,
                    Name = brand.Name,
                    Order = brand.Order
                };
            }

            return null;
        }

        public PagedProductDto GetProducts(ProductFilter filter)
        {
            var query = _context.Products.Include("Brand").Include("Section").AsQueryable();

            if (filter.Ids != null && filter.Ids.Count > 0)
            {
                query = query.Where(q => filter.Ids.Contains(q.Id));
            }

            if (filter.BrandId.HasValue)
                query = query.Where(c => c.BrandId.HasValue && c.BrandId.Value.Equals(filter.BrandId.Value));

            if (filter.SectionId.HasValue)
                query = query.Where(c => c.SectionId.Equals(filter.SectionId.Value));

            query = query.Where(c => c.IsDelete == false);

            var model = new PagedProductDto
            {
                TotalCount = query.Count()
            };

            if (filter.PageSize.HasValue)
            {
                model.Products = query.OrderBy(c => c.Order).Skip((filter.Page - 1) * filter.PageSize.Value).Take(filter.PageSize.Value)
                    .Select(p =>
                        new ProductDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Order = p.Order,
                            Price = p.Price,
                            ImageUrl = p.ImageUrl,
                            Brand = p.BrandId.HasValue ? new BrandDto() { Id = p.Brand.Id, Name = p.Brand.Name } : null,
                            Section = new SectionDto() { Id = p.SectionId, Name = p.Section.Name }
                        }).ToList();
            }
            else
            {
                model.Products = query.OrderBy(c => c.Order).Select(p =>
                    new ProductDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Order = p.Order,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                        Brand = p.BrandId.HasValue ? new BrandDto() { Id = p.Brand.Id, Name = p.Brand.Name } : null,
                        Section = new SectionDto() { Id = p.SectionId, Name = p.Section.Name }
                    }).ToList();
            }
            return model;
        }

        public ProductDto GetProductById(int id)
        {
            var product = _context.Products.Include("Brand").Include("Section").FirstOrDefault(p => p.Id.Equals(id));
            if (product == null) return null;

            var dto = new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Order = product.Order,
                Price = product.Price,
                Section = new SectionDto() { Id = product.SectionId, Name = product.Section.Name, ParentId = product.Section.ParentId, Order = product.Section.Order }
            };

            if (product.Brand != null)
                dto.Brand = new BrandDto()
                {
                    Id = product.Brand.Id,
                    Name = product.Brand.Name,
                    Order = product.Brand.Order
                };

            return dto;
        }

        public SaveResult CreateProduct(ProductDto productDto)
        {
            try
            {
                var product = new Product()
                {
                    BrandId = productDto.Brand?.Id,
                    SectionId = productDto.Section.Id,
                    Name = productDto.Name,
                    ImageUrl = productDto.ImageUrl,
                    Order = productDto.Order,
                    Price = productDto.Price
                };
                _context.Products.Add(product);
                _context.SaveChanges();
                return new SaveResult
                {
                    IsSuccess = true
                };
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new SaveResult
                {
                    IsSuccess = false,
                    Errors = new List<string>()
                    {
                        ex.Message
                    }
                };
            }
            catch (DbUpdateException ex)
            {
                return new SaveResult
                {
                    IsSuccess = false,
                    Errors = new List<string>()
                    {
                        ex.Message
                    }
                };
            }
            catch (Exception e)
            {
                return new SaveResult
                {
                    IsSuccess = false,
                    Errors = new List<string>()
                    {
                        e.Message
                    }
                };
            }
        }

        public SaveResult UpdateProduct(ProductDto productDto)
        {
            var product = _context.Products.FirstOrDefault(c => c.Id == productDto.Id);

            if (product == null)
            {
                return new SaveResult()
                {
                    IsSuccess = false,
                    Errors = new List<string>() { "Entity not exist" }
                };
            }

            product.BrandId = productDto.Brand.Id;
            product.SectionId = productDto.Section.Id;
            product.ImageUrl = productDto.ImageUrl;
            product.Order = productDto.Order;
            product.Price = productDto.Price;
            product.Name = productDto.Name;
            try
            {
                _context.SaveChanges();
                return new SaveResult
                {
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new SaveResult
                {
                    IsSuccess = false,
                    Errors = new List<string>()
                    {
                        e.Message
                    }
                };
            }
        }

        public SaveResult DeleteProduct(int productId)
        {
            var product = _context.Products.FirstOrDefault(c => c.Id == productId);

            if (product == null)
            {
                return new SaveResult()
                {
                    IsSuccess = false,
                    Errors = new List<string>() { "Entity not exist" }
                };
            }

            try
            {
                //_context.Remove(product);
                product.IsDelete = true;
                _context.SaveChanges();
                return new SaveResult()
                {
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new SaveResult
                {
                    IsSuccess = false,
                    Errors = new List<string>()
                    {
                        e.Message
                    }
                };
            }
        }
    }
}
