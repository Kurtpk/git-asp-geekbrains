using System.Collections.Generic;

namespace WebStore.DomainNew.Models.Product
{
    public class BrandCompleteViewModel
    {
        public IEnumerable<BrandViewModel> Brands { get; set; }
        public int? CurrentBrandId { get; set; }
    }
}
