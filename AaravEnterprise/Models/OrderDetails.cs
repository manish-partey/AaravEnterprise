using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AaravEnterprise.Models
{
    public class OrderDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("OrderId")]
        [ValidateNever]
        public int OrderId { get; set; }      
        public int ServiceId { get; set; }       
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public Order Order { get; set; }
    }
}
