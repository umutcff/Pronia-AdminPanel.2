using System.ComponentModel.DataAnnotations;

namespace ProniaUmut.Models
{
    public class ShippingItem
    {
        public int Id { get; set; }

        [MinLength(3)]
        [MaxLength(32)]
        public string Title { get; set; }
        public string? Description { get; set; }

        [Required]
        public string ImagePath {  get; set; }
    }
}
