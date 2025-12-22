using Microsoft.AspNetCore.Mvc;
using ProniaUmut.Contexts;

namespace ProniaUmut.Areas.Admin.Controllers
{
[Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        { 

            return View("Index");
        }

    }
}
