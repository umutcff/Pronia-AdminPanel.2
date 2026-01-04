using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ProniaUmut.ViewModels.ProductViewModel
{
    public class ProductUpdateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? MainImage { get; set; }
        public IFormFile? HoverImage { get; set; }
        public List<IFormFile>? Images { get; set; }
        public string MainImagePath { get; set; }
        public string HoverImagePath { get; set; }
        public List<int> TagIds { get; set; } = [];
        public List<string>? AdditionalImagePaths { get; set; } = [];
        public List<int> AdditionalImageIds { get; set; } = [];
    }
}

