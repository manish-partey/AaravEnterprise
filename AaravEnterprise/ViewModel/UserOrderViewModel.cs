using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AaravEnterprise.ViewModel
{
    public class UserOrderViewModel
    {
        public string UserId { get; set; }
        public string CustName { get; set; }
        public string CustEmail { get; set; }
        public string CustPhoneNumber { get; set; }
        public double TotalAmount { get; set; }
    }
}
