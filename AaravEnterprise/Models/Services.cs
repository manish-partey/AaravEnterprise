using System.ComponentModel.DataAnnotations;

namespace AaravEnterprise.Models
{
    public class Services
    {
        [Key]
        public int Id { get; set; }
        public string ServiceTitle { get; set; }
        public string ServiceDescription { get; set; }

    }
}
