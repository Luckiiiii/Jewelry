using Jewelry.Data.Entities;
using System.Collections.Generic;

namespace Jewelry.Models
{
    public class AddInventoryReceiptViewModel
    {
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int MaterialId { get; set; }
        public int PurchasePrice { get; set; }
        public int Quantity { get; set; }
    }

}

