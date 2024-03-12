using Microsoft.Identity.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jewelry.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
        //public DateTime? CancellationDate { get; set; }
        //public string? CancellationReason { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public StoreUser User { get; set; }
        public Payments PaymentMethod { get; set; }
        public string DeliveryAddress { get; set; }
        public string? Note { get; set; }
        public string PhoneNumber {  get; set; }
        public ICollection<Status> Status { get; set; }
        public string ConsigneeName { get; set; }
    }
}
