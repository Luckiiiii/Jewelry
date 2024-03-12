using Jewelry.Data.Entities;

namespace Jewelry.Models
{
    public class AddProductViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string WarrantyInformation { get; set; }
        public ICollection<IFormFile> Images { get; set; }
        public ProductCategory Category { get; set; }
        public IEnumerable<ProductCategory>? Categories { get; set; }
    }
}
