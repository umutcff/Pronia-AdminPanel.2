using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaUmut.Contexts;
using ProniaUmut.Models;
using ProniaUmut.ViewModels.ProductViewModel;

namespace ProniaUmut.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoValidateAntiforgeryToken]

    public class ProductController(AppDBContext _context, IWebHostEnvironment _envoriment) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(x => x.Category).ToListAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await SendCategories();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            await SendCategories();

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (!vm.MainImage.ContentType.Contains("image"))
            {
                ModelState.AddModelError("MainImage", "Yalniz sekil daxil ede bilersen!");
                return View(vm);
            }

            if (vm.MainImage.Length > 3 * 1024 * 1024)
            {
                ModelState.AddModelError("MainImage", "Sekil 3-Mb-dan artiq ola bilmez!");
                return View(vm);
            }

            if (!vm.HoverImage.ContentType.Contains("image"))
            {
                ModelState.AddModelError("HoverImage", "Yalniz sekil daxil ede bilersen!");
                return View(vm);
            }

            if (vm.HoverImage.Length > 3 * 1024 * 1024)
            {
                ModelState.AddModelError("HoverImage", "Sekil 3-Mb-dan artiq ola bilmez!");
                return View(vm);
            }

            string uniqueMainPath = Guid.NewGuid().ToString() + vm.MainImage.FileName;
            string mainPath = $@"{_envoriment.WebRootPath}/assets/images/website-images/{uniqueMainPath}";
            FileStream mainStream = new FileStream(mainPath, FileMode.Create);
            await vm.MainImage.CopyToAsync(mainStream);


            string uniqueHoverPath = Guid.NewGuid().ToString() + vm.HoverImage.FileName;
            string hoverPath = $@"{_envoriment.WebRootPath}/assets/images/website-images/{uniqueHoverPath}";
            FileStream hoverStream = new FileStream(hoverPath, FileMode.Create);
            await vm.HoverImage.CopyToAsync(hoverStream);


            Product product = new()
            {
                Name = vm.Name,
                Description = vm.Description,
                CategoryId = vm.CategoryId,
                Price = vm.Price,
                MainImagePath = uniqueMainPath,
                HoverImagePath = uniqueHoverPath,
            };

            await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedProduct = await _context.Products.FindAsync(id);

            if (deletedProduct == null)
            {
                return NotFound();
            }

            _context.Products.Remove(deletedProduct);

            string folderPath = Path.Combine(_envoriment.WebRootPath, "assets", "images", "web-images");
            string deleteMainPath = Path.Combine(folderPath, deletedProduct.MainImagePath);
            string deleteHoverPath = Path.Combine(folderPath, deletedProduct.HoverImagePath);
            if (System.IO.File.Exists(deleteMainPath))
            {
                System.IO.File.Delete(deleteMainPath);
            }

            if (System.IO.File.Exists(deleteHoverPath))
            {
                System.IO.File.Delete(deleteHoverPath);
            }


            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            await SendCategories();
            var updatedProduct = await _context.Products.FindAsync(id);
            if (updatedProduct == null)
            {
                return NotFound();
            }
            return View(updatedProduct);
        }


        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            await SendCategories();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var existProduct = await _context.Products.FindAsync(product.Id);
            if (existProduct == null)
            {
                return BadRequest();
            }

            existProduct.Name = product.Name;
            existProduct.Description = product.Description;
            /*existProduct.ImagePath = product.ImagePath;*/
            existProduct.Price = product.Price;
            existProduct.CategoryId = product.CategoryId;

            _context.Products.Update(existProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }




        private async Task SendCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;
        }
    }
}
