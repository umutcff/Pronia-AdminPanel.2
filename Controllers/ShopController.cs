using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaUmut.Contexts;

namespace ProniaUmut.Controllers
{
    public class ShopController(AppDBContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products=await _context.Products.ToListAsync();
            return View(products);
        }
    }
}
