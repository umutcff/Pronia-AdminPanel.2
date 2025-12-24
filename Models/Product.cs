using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ProniaUmut.Models.Common;

namespace ProniaUmut.Models
{
    public class Product:BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ImagePath {  get; set; }
        [Precision(10,2)]
        [Required]
        public decimal Price { get; set; }
        public Category? Category { get; set; }
        [Required]
        public int CategoryId { get; set; }

    }
}
