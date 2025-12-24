using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaUmut.Contexts;
using ProniaUmut.Models;

namespace ProniaUmut.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoValidateAntiforgeryToken]

    public class ProductController(AppDBContext _context) : Controller
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
        public async Task<IActionResult> Create(Product product)
        {
            await SendCategories();

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            await _context.AddAsync(product)
                ;
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
            existProduct.ImagePath = product.ImagePath;
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
