using System.Collections.Generic;

namespace WebStore.DomainNew.Models.Product
{
    public class CatalogViewModel
    {
        public int? BrandId { get; set; }
        public int? SectionId { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }
}
