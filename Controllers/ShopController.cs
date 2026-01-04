using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaUmut.Contexts;
using ProniaUmut.ViewModels.ProductViewModel;

namespace ProniaUmut.Controllers
{
    public class ShopController(AppDBContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }




        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.Products.Select(x => new ProductGetVM()
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Description = x.Description,
                CategoryName = x.Category.Name,
                AdditionalImagePaths = x.ProductImages.Select(x => x.ImagePath).ToList(),
                HoverImagePath = x.HoverImagePath,
                MainImagePath = x.MainImagePath,
                TagNames = x.ProductTags.Select(x => x.Tag.Name).ToList(),
            }).FirstOrDefaultAsync(x => x.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
