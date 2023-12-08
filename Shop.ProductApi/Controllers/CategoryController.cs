using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.ProductApi.Context;
using Shop.ProductApi.Models;

namespace Shop.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetById (int id)
        {
            var category = await _context.Categories.Where(cat => cat.Id == id).SingleOrDefaultAsync();
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> PostCategories(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return Ok(category);
        }

        [HttpPut]
        public async Task<ActionResult<Category>> PutCategories (Category category)
        {
            _context.Update(category);
            await _context.SaveChangesAsync();
            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task DeleteCategories (int id)
        {
            var deleted = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(deleted);
            _context.SaveChanges();
            //return Ok(category);
        }
        
    }
}
