using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Shop.Web.Models;
using Shop.Web.Services.Contracts;

namespace Shop.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _service;
        public CategoriesController(ICategoryService service)
        {
            _service = service;   
        }
        public async Task<ActionResult<IEnumerable<Category>>> Index()
        {
            var cat = await _service.Get();
            return View(cat);
        }
        [HttpGet]
        public async Task<ActionResult> CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateCategory(Category category)
        {
            await _service.Post(category);
            return View(category);
        }
        [HttpGet]
        public async Task<ActionResult> UpdateCategory(int id)
        {
            var result = await _service.GetById(id);

            if (result is null)
                return View("Error");

            return View(result);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateCategory(Category category)
        {
            await _service.Put(category);
            return View(category);
        }
        [HttpGet]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var result = await _service.GetById(id);

            if (result is null)
                return View("Error");

            return View(result);
        }

        public async Task<ActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
