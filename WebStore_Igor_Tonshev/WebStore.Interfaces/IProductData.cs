using System.Collections.Generic;
using WebStore.DomainNew.Dto;
using WebStore.DomainNew.Dto.Product;
using WebStore.DomainNew.Entities;
using WebStore.DomainNew.Filters;

namespace WebStore.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с товарами
    /// </summary>
    public interface IProductData
    {
        /// <summary>
        /// Список секций
        /// </summary>
        /// <returns></returns>
        IEnumerable<SectionDto> GetSections();

        /// <summary>
        /// Секция по Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        SectionDto GetSectionById(int id);

        /// <summary>
        /// Список брендов
        /// </summary>
        /// <returns></returns>
        IEnumerable<BrandDto> GetBrands();

        /// <summary>
        /// Бренд по Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        BrandDto GetBrandById(int id);

        /// <summary>
        /// Список товаров
        /// </summary>
        /// <param name="filter">Фильтр товаров</param>
        /// <returns></returns>
        PagedProductDto GetProducts(ProductFilter filter);

        /// <summary>
        /// Продукт
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Сущность Product, если нашёл, иначе null</returns>
        ProductDto GetProductById(int id);

        /// <summary>
        /// Создать продукт
        /// </summary>
        /// <param name="product">Сущность Product</param>
        /// <returns></returns>
        SaveResult CreateProduct(ProductDto product);

        /// <summary>
        /// Обновить продукт
        /// </summary>
        /// <param name="product">Сущность Product</param>
        /// <returns></returns>
        SaveResult UpdateProduct(ProductDto product);

        /// <summary>
        /// Удалить продукт
        /// </summary>
        /// <param name="productId">Id продукта</param>
        /// <returns></returns>
        SaveResult DeleteProduct(int productId);
    }
}
