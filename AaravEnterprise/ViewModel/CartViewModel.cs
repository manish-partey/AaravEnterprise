using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AaravEnterprise.ViewModel
{
    public class CartViewModel
    {
        public int CartId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceTitle { get; set; }        
        public string PackageTitle { get; set; }
        public int Amount { get; set; }
    }
}
