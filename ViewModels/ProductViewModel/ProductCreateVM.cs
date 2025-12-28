using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ProniaUmut.Models;

namespace ProniaUmut.ViewModels.ProductViewModel
{
    public class ProductCreateVM
    {
       public int Id { get; set; }
       public string Name { get; set; }
       [Required]
       public string Description { get; set; }

       [Precision(10, 2)]
       [Required]
       public decimal Price { get; set; }

       [Required]
       public int CategoryId { get; set; }

       public List<int> TagIds { get; set; }
       public IFormFile MainImage { get; set; }
       public IFormFile HoverImage { get; set; }
       public List<IFormFile>? AdditionalImage { get; set; }
    }
}
