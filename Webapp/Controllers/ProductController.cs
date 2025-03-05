using Microsoft.AspNetCore.Mvc;
using Webapp.Models;
using Webapp.Data; // Asegúrate de agregar el espacio de nombres para tu DbContext
using Microsoft.EntityFrameworkCore;

namespace Webapp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action para listar productos
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);  // Pasamos la lista de productos a la vista
        }

        // Action para agregar un nuevo producto (GET)
        public IActionResult Create()
        {
            return View();  // Devuelve la vista de Create.cshtml
        }

        // Action para agregar un nuevo producto (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);  // Agrega el producto a la base de datos
                await _context.SaveChangesAsync();  // Guarda los cambios
                return RedirectToAction(nameof(Index));  // Redirige a la vista de productos
            }
            return View(product);  // Si hay errores, vuelve a mostrar la vista con el modelo
        }

        // Action para editar un producto (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();  // Si no hay id, devuelve NotFound
            }

            var product = await _context.Products.FindAsync(id);  // Busca el producto por id
            if (product == null)
            {
                return NotFound();  // Si no se encuentra, devuelve NotFound
            }
            return View(product);  // Devuelve la vista con el producto a editar
        }

        // Action para editar un producto (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();  // Si el id no coincide, devuelve NotFound
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);  // Actualiza el producto
                    await _context.SaveChangesAsync();  // Guarda los cambios
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(e => e.ProductId == product.ProductId))
                    {
                        return NotFound();  // Si no se encuentra el producto, devuelve NotFound
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));  // Redirige a la vista de productos
            }
            return View(product);  // Si el modelo es inválido, vuelve a mostrar el formulario de edición
        }

        // Action para eliminar un producto (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();  // Si no hay id, devuelve NotFound
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);  // Busca el producto por id
            if (product == null)
            {
                return NotFound();  // Si no se encuentra, devuelve NotFound
            }

            return View(product);  // Devuelve la vista de eliminación con el producto a eliminar
        }

        // Action para eliminar un producto (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);  // Busca el producto por id
            _context.Products.Remove(product);  // Elimina el producto
            await _context.SaveChangesAsync();  // Guarda los cambios
            return RedirectToAction(nameof(Index));  // Redirige a la vista de productos
        }
    }
}
