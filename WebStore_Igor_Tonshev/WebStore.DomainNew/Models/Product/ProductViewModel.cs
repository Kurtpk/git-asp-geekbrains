using System.ComponentModel.DataAnnotations;
using WebStore.DomainNew.Entities.Base.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebStore.DomainNew.Models.Product
{
    public class ProductViewModel : INamedEntity, IOrderedEntity
    {
        public int Id { get; set; }

        [Required, Display(Name = "Название")]
        public string Name { get; set; }

        [Required, Display(Name = "Порядок")]
        public int Order { get; set; }

        [Required, Display(Name = "Изображение")]
        public string ImageUrl { get; set; }

        [Required, Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Display(Name = "Бренд")]
        public string Brand { get; set; }

        public int? BrandId { get; set; }

        [Display(Name = "Категория")]
        public string Section { get; set; }

        [Required]
        public int SectionId { get; set; }

        public SelectList Sections { get; set; }

        public SelectList Brands { get; set; }
    }
}
