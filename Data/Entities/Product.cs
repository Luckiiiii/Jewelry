using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jewelry.Data.Entities
{
  public class Product
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<ProductImage> Img { get; set; }
    public ICollection<ProductItem>? Item { get; set; }
    public ProductCategory Category { get; set; }
    public string WarrantyInformation { get; set; }
  }
}
