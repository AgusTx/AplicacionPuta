using Microsoft.AspNetCore.Mvc;
using Webapp.Data;
using Webapp.Models;
using System.Linq;
using Webapp.ViewModels;


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
            var categories = _context.Categories.ToList();
            var availableProducts = _context.Products.Where(p => p.Stock > 0).ToList(); // Solo productos con stock

            var viewModel = new HomeViewModel
            {
                Categories = categories,
                Products = availableProducts
            };

            return View(viewModel);
        }
        [HttpPost]
        public JsonResult BuyProduct([FromBody] BuyProductRequest request)
        {
            if (request == null || request.ProductId <= 0)
            {
                return Json(new { success = false, message = "ID de producto inválido." });
            }

            var product = _context.Products.FirstOrDefault(p => p.ProductId == request.ProductId);

            if (product == null)
            {
                return Json(new { success = false, message = "Producto no encontrado." });
            }

            if (product.Stock <= 0)
            {
                return Json(new { success = false, message = "Producto sin stock." });
            }

            product.Stock--; // Reducir stock
            _context.SaveChanges();

            return Json(new { success = true, newStock = product.Stock });
        }

        [HttpPost]
        public JsonResult AddProduct([FromBody] Product product)
        {
            if (product == null || string.IsNullOrEmpty(product.Name) || product.Price <= 0 || product.Stock < 0)
                return Json(new { success = false, message = "Datos inválidos." });

            _context.Products.Add(product);
            _context.SaveChanges();

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult EditProduct([FromBody] Product updatedProduct)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == updatedProduct.ProductId);
            if (product == null)
                return Json(new { success = false, message = "Producto no encontrado." });

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            product.Stock = updatedProduct.Stock;

            _context.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult DeleteProduct([FromBody] BuyProductRequest request)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == request.ProductId);
            if (product == null)
                return Json(new { success = false, message = "Producto no encontrado." });

            _context.Products.Remove(product);
            _context.SaveChanges();

            return Json(new { success = true });
        }

        //[HttpPost]
        //public IActionResult AddProduct([FromBody] Product product)
        //{
        //    if (product != null && !string.IsNullOrEmpty(product.Name) && product.Price > 0 && product.Stock >= 0)
        //    {
        //        _context.Products.Add(product);
        //        _context.SaveChanges();

        //        return Json(new { success = true, productId = product.ProductId });
        //    }

        //    return Json(new { success = false });
        //}
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

    
}
