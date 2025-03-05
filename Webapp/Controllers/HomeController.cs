using Microsoft.AspNetCore.Mvc;
using Webapp.Data;
using Webapp.Models;
using System.Linq;

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
            var categories = _context.Categories.ToList(); // Obtener categorías
            var availableProducts = _context.Products.Where(p => p.Stock > 0).ToList(); // Solo productos con stock

            var viewModel = new HomeViewModel
            {
                Categories = categories,
                Products = availableProducts
            };

            return View(viewModel);
        }

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

        [HttpPost]
        public JsonResult CreateAjax(string name, decimal price, int stock)
        {
            var product = new Product { Name = name, Price = price, Stock = stock };
            _context.Products.Add(product);
            _context.SaveChanges();

            return Json(new { productId = product.ProductId, name = product.Name, price = product.Price, stock = product.Stock });
        }

        public IActionResult ProductList()
        {
            var products = _context.Products.ToList();
            return PartialView("_ProductList", products);
        }
    }

    public class HomeViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
    }
}
