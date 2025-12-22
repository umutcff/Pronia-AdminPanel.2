using Microsoft.AspNetCore.Mvc;
using ProniaUmut.Contexts;

namespace ProniaUmut.Controllers
{
    public class HomeController:Controller
    {

        private readonly AppDBContext _context;
        public HomeController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var shippingItems=_context.ShippingItems.ToList();
           
            return View(shippingItems);
        }
    }
}
