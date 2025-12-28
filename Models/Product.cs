using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ProniaUmut.Models.Common;

namespace ProniaUmut.Models
{
    public class Product : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        [Precision(10, 2)]
        [Required]
        public decimal Price { get; set; }
        public Category Category { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string MainImagePath { get; set; }
        public string HoverImagePath { get; set; }


        public ICollection<ProductImage> ProductImages { get; set; } = [];
        public ICollection<ProductTag> ProductTags { get; set; } = [];
    }



}
