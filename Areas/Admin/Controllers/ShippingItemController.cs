using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProniaUmut.Contexts;
using ProniaUmut.Models;

namespace ProniaUmut.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoValidateAntiforgeryToken]
    public class ShippingItemController : Controller
    {
        private readonly AppDBContext _context;
        public ShippingItemController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var items=await _context.ShippingItems.ToListAsync();
            return View(items);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(ShippingItem shippItem)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            
            await _context.ShippingItems.AddAsync(shippItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(int id)
        {
            var deleted= await _context.ShippingItems.FindAsync(id);

            if (deleted == null)
            {
                return NotFound();
            }
            
            _context.ShippingItems.Remove(deleted);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var updated = await _context.ShippingItems.FindAsync(id);

            if (updated == null)
            {
                return NotFound();
            }
            return View(updated);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ShippingItem shippingItem)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var existItem= await _context.ShippingItems.FindAsync(shippingItem.Id);

            if(existItem == null)
            {
                return NotFound();
            }

            existItem.Title = shippingItem.Title;
            existItem.Description = shippingItem.Description;
            existItem.ImagePath = shippingItem.ImagePath;

            _context.ShippingItems.Update(existItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));     
        }

    }
}
