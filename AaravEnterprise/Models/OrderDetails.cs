using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AaravWebSolution.Models
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }
        public Order Order { get; set; }
        public Services Service { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
    }
}
