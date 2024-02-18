using System.ComponentModel.DataAnnotations;
using System;
using Jewelry.Data.Entities;

namespace Jewelry.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }

        public DateTime OrdrDate{ get; set; }
        [Required]
        [MinLength(4)]
        public string OrderNumber { get; set; }
        public DateTime? CancellationDate { get; set; }
        public string? CancellationReason { get; set; }
        public ICollection<OrderItemViewModel> Items { get; set; }
        public Payments? PaymentMethod { get; set; }
        public string? CustomerName { get; set; }
        public string? EmployeeName { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
        public string? DeliveryImage { get; set; }
        public Status? Status { get; set; }
    }
}
