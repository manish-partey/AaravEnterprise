using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AaravEnterprise.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        [ValidateNever]
        public Services Service { get; set; }
        public int PackageId { get; set; }
        [ForeignKey("PackageId")]
        [ValidateNever]
        public Package Package { get; set; }
        public int Amount { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
