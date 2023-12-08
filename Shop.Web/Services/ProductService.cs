using Shop.Web.Models;
using Shop.Web.Services.Contracts;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;

namespace Shop.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private IEnumerable<Product> Products { get; set; }
        private Product Product { get; set; }

        private readonly JsonSerializerOptions _options;

        public ProductService(IHttpClientFactory client)
        {
            _httpClientFactory = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }


        public async Task<IEnumerable<Product>> Get()
        {
            var httpClient = _httpClientFactory.CreateClient("ProductApi");
            var httpResponseMessage = await httpClient.GetAsync(
                "api/Product");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                Products = await JsonSerializer.DeserializeAsync
                    <IEnumerable<Product>>(contentStream, _options);
            }
            return Products;
        }

        public async Task<Product> GetById(int id, string token)
        {
            var httpClient = _httpClientFactory.CreateClient("ProductApi");

            httpClient.DefaultRequestHeaders.Authorization =
new AuthenticationHeaderValue("Bearer", token);

            var httpResponseMessage = await httpClient.GetAsync(
                $"api/Product/" + id);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                Product = await JsonSerializer.DeserializeAsync
                    <Product>(contentStream, _options);
            }
            return Product;
        }

        public async Task<Product> Post(Product Product, string token)
        {

            var httpClient = _httpClientFactory.CreateClient("ProductApi");

            httpClient.DefaultRequestHeaders.Authorization =
new AuthenticationHeaderValue("Bearer", token);

            var newProduct = new StringContent(
                JsonSerializer.Serialize(Product),
                Encoding.UTF8,
                Application.Json); // using static System.Net.Mime.MediaTypeNames;

            using var httpResponseMessage =
                await httpClient.PostAsync("/api/Product/", newProduct);

            httpResponseMessage.EnsureSuccessStatusCode();
            return Product;
        }

        public async Task<Product> Put(Product Product, string token)
        {
            var httpClient = _httpClientFactory.CreateClient("ProductApi");

            httpClient.DefaultRequestHeaders.Authorization =
new AuthenticationHeaderValue("Bearer", token);

            var ProductJson = new StringContent(
                JsonSerializer.Serialize(Product),
                Encoding.UTF8,
                Application.Json);

            using var httpResponseMessage =
                await httpClient.PutAsync($"/api/Product/", ProductJson);

            httpResponseMessage.EnsureSuccessStatusCode();
            return Product;
        }

        public async Task Delete(int id)
        {
            var _httpClient = _httpClientFactory.CreateClient("ProductApi");
            using var httpResponseMessage =
                await _httpClient.DeleteAsync($"/api/Product/{id}");

            httpResponseMessage.EnsureSuccessStatusCode();
        }
    }
}
