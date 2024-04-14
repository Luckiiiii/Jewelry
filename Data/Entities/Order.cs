using Microsoft.Identity.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Jewelry.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public string UserId { get; set; }
        public StoreUser User { get; set; }
        public int PaymentMethodId { get; set; }
        public Payments PaymentMethod { get; set; }
        public string DeliveryAddress { get; set; }
        public string? Note { get; set; }
        public string PhoneNumber {  get; set; }
        public ICollection<Status> Status { get; set; }
        public string ConsigneeName { get; set; }
    }
}
