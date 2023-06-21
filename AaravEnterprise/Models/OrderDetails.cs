using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AaravEnterprise.Models
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }
        [ValidateNever]
        public int ? OrderId { get; set; }
        [ValidateNever]
        public int ? ServiceId { get; set; }       
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
    }
}
