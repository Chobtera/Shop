using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Shop.Web.Models;
using Shop.Web.Services.Contracts;

namespace Shop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _service;
        public ProductsController(IProductService service)
        {
            _service = service;
        }
        public async Task<ActionResult<IEnumerable<Product>>> Index()
        {
            var cat = await _service.Get();
            return View(cat);
        }
        [HttpGet]
        public async Task<ActionResult> CreateProduct()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateProduct(Product product)
        {
            string token = await HttpContext.GetTokenAsync("access_token");
            await _service.Post(product, token);
            return View(product);
        }
        [HttpGet]
        public async Task<ActionResult> UpdateProduct(int id)
        {
            string token = await HttpContext.GetTokenAsync("access_token");
            var result = await _service.GetById(id, token);

            if (result is null)
                return View("Error");

            return View(result);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateProduct(Product product)
        {
            string token = await HttpContext.GetTokenAsync("access_token");
            await _service.Put(product, token);
            return View(product);
        }
        [HttpGet]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            string token = await HttpContext.GetTokenAsync("access_token");
            var result = await _service.GetById(id, token);

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
