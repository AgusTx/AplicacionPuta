using Microsoft.AspNetCore.Mvc;
using Webapp.Data;
using Webapp.Models;
using System.Linq;

namespace Webapp.Controllers
{


    public class HomeController : Controller
    {

        [HttpPost]
        public IActionResult AddProduct([FromBody] Product product)
        {
            if (product != null && !string.IsNullOrEmpty(product.Name) && product.Price > 0)
            {
                _context.Products.Add(product);
                _context.SaveChanges();

                return Json(new { success = true, productId = product.ProductId });
            }
            return Json(new { success = false });
        }

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList(); // Obtener categorías
            var products = _context.Products.ToList(); // Obtener productos

            var viewModel = new HomeViewModel
            {
                Categories = categories,
                Products = products
            };

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult CreateAjax(string name, decimal price)
        {
            var product = new Product { Name = name, Price = price };
            _context.Products.Add(product);
            _context.SaveChanges();

            return Json(new { productId = product.ProductId, name = product.Name, price = product.Price });
        }
    }

    public class HomeViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
    }
}
