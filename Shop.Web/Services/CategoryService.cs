using Microsoft.AspNetCore.Http.HttpResults;
using Shop.Web.Models;
using Shop.Web.Services.Contracts;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Shop.Web.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private IEnumerable<Category> Categories { get; set; }
        private Category Category { get; set; }

        private readonly JsonSerializerOptions _options;

        public CategoryService(IHttpClientFactory client)
        {
            _httpClientFactory = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }


        public async Task<IEnumerable<Category>> Get()
        {
            var httpClient = _httpClientFactory.CreateClient("ProductApi");
            var httpResponseMessage = await httpClient.GetAsync(
                "api/category");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                Categories = await JsonSerializer.DeserializeAsync
                    <IEnumerable<Category>>(contentStream, _options);
            }
            return Categories;
        }

        public async Task<Category> GetById(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ProductApi");
            var httpResponseMessage = await httpClient.GetAsync(
                $"api/category/" + id);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                Category = await JsonSerializer.DeserializeAsync
                    <Category>(contentStream, _options);
            }
            return Category;
        }

        public async Task<Category> Post(Category category)
        {

            var httpClient = _httpClientFactory.CreateClient("ProductApi");
            var newcategory = new StringContent(
                JsonSerializer.Serialize(category),
                Encoding.UTF8,
                Application.Json); // using static System.Net.Mime.MediaTypeNames;

            using var httpResponseMessage =
                await httpClient.PostAsync("/api/category/", newcategory);

            httpResponseMessage.EnsureSuccessStatusCode();
            return category;
        }

        public async Task<Category> Put(Category category)
        {
            var _httpClient = _httpClientFactory.CreateClient("ProductApi");
            var categoryJson = new StringContent(
                JsonSerializer.Serialize(category),
                Encoding.UTF8,
                Application.Json);

            using var httpResponseMessage =
                await _httpClient.PutAsync($"/api/category/", categoryJson);

            httpResponseMessage.EnsureSuccessStatusCode();
            return category;
        }

        public async Task Delete(int id)
        {
            var _httpClient = _httpClientFactory.CreateClient("ProductApi");
            using var httpResponseMessage =
                await _httpClient.DeleteAsync($"/api/category/{id}");

            httpResponseMessage.EnsureSuccessStatusCode();
        }
    }
}
