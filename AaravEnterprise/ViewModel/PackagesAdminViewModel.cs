using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AaravEnterprise.ViewModel
{
    public class PackagesAdminViewModel
    {
        public int Id { get; set; }
        public string ServiceTitle { get; set; }
        public string ServiceDesciption { get; set; }
        public int PackageId { get; set; }
        public string PackageTitle { get; set; }
        public string PackageDescription { get; set; }
        public int Price { get; set; }
    }
}
