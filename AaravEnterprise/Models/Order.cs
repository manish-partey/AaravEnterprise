using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AaravEnterprise.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? PaymentId { get; set; }
    }
}
