using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.ProductApi.Context;
using Shop.ProductApi.Models;

namespace Shop.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _context.Products.Where(cat => cat.Id == id).SingleOrDefaultAsync();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProducts(Product product)
        {
            await _context.Products.AddAsync(product);
            _context.SaveChanges();
            return Ok(product);
        }

        [HttpPut]
        public async Task<ActionResult<Product>> PutProducts(Product product, int id)
        {
            //var Productid = await _context.Categories.Where(cat => cat.Id == id).SingleOrDefaultAsync();
            _context.Update(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProducts(int id)
        {
            var deleted = _context.Products.Find(id);
            _context.Products.Remove(deleted);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
