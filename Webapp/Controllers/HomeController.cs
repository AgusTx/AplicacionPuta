using Microsoft.AspNetCore.Mvc;
using Webapp.Data;

namespace Webapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList(); // 🔹 Obtener datos de la base de datos
            return View(categories);
        }
    }
}