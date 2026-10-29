using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.Filters;
using ProductManagement.Filters.Filters;
using ProductManagement.Models;
namespace ProductManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(LogActionFilter))]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;               // EF
        private readonly AdoProductRepository _ado;      // ADO.NET

        public ProductsController(AppDbContext db, AdoProductRepository ado)
        {
            _db = db;
            _ado = ado;
        }

        // ---------- EF Core actions ----------
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _db.Products.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var p = await _db.Products.FindAsync(id);
            return p == null ? NotFound() : Ok(p);
        }

        [HttpPost]
        [ValidateProductFilter]
        public async Task<IActionResult> Create(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [ValidateProductFilter]
        public async Task<IActionResult> Update(int id, Product product)
        {
            var existing = await _db.Products.FindAsync(id);
            if (existing == null) return NotFound();
            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Stock = product.Stock;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.Products.FindAsync(id);
            if (p == null) return NotFound();
            _db.Products.Remove(p);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // ---------- ADO.NET actions ----------
        [HttpGet("low-stock/{threshold}")]
        public IActionResult LowStock(int threshold)
            => Ok(_ado.GetLowStock(threshold));

        [HttpGet("inventory-value")]
        public IActionResult InventoryValue()
            => Ok(new { totalValue = _ado.GetTotalInventoryValue() });
    }
}