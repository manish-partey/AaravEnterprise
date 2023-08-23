using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AaravEnterprise.ViewModel
{
    public class InCompleteOrderViewModel
    {
        public int InvoiceId { get; set; }
        public int OrderId { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal Amount { get; set; }
        public string ServiceTitle { get; set; }
        public double Price { get; set; }
    }
}
