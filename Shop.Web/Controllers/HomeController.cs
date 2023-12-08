using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Web.Models;
using Shop.Web.Services;
using Shop.Web.Services.Contracts;
using System.Diagnostics;

namespace Shop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _service;
        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger, IProductService service, ICartService cartservice)
        {
            _logger = logger;
            _service = service;
            _cartService = cartservice;
        }

        public async Task <ActionResult<IEnumerable<Product>>> Index()
        {
            var cat = await _service.Get();
            return View(cat);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Product>> ProductDetails(int id)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var product = await _service.GetById(id, token);

            if (product is null)
                return View("Error");

            return View(product);
        }

        [HttpPost]
        [ActionName("ProductDetails")]
        [Authorize]
        public async Task<ActionResult<Product>> ProductDetailsPost
            (Product productVM)
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            Cart cart = new()
            {
                CartHeader = new CartHeader
                {
                    UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value
                }
            };

            CartItem cartItem = new()
            {
                Quantity = productVM.Stock,
                ProductId = productVM.Id,
                Product = await _service.GetById(productVM.Id, token)
            };

            List<CartItem> cartItemsVM = new List<CartItem>();
            cartItemsVM.Add(cartItem);
            cart.CartItems = cartItemsVM;

            var result = await _cartService.AddItemToCartAsync(cart, token);

            if (result is not null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(productVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}