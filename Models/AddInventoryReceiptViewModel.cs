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
        public int PurityId { get; set; }
        public double Weight { get; set; }
        public decimal PurchasePrice { get; set; }
        public int Quantity { get; set; }
    }

}

