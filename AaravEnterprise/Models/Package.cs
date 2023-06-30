using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AaravEnterprise.Models
{
    public class Package
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PackageTitle { get; set; }
        public string PackageDescription { get; set; }
        public int Price { get; set; }
        public int ServicesId { get; set; }
        [ForeignKey("ServicesId")]
        public Services Services { get; set; }


    }
}
