using System.ComponentModel.DataAnnotations;

namespace AaravEnterprise.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)] // Adjust the length as needed
        public string Name { get; set; }
    }
}
