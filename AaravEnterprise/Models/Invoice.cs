using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AaravEnterprise.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        public int OrderId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal Amount { get; set; }
        public Order Order { get; set; }
    }
}
