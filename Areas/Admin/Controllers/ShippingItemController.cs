using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProniaUmut.Contexts;
using ProniaUmut.Models;

namespace ProniaUmut.Areas.Admin.Controllers
{
    [Area("Admin")]
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
    }
}
