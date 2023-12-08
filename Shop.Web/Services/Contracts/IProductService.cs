using Shop.Web.Models;

namespace Shop.Web.Services.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> Get();
        Task<Product> GetById(int id, string token);
        Task<Product> Post(Product product, string token);
        Task<Product> Put(Product product, string token);
        Task Delete(int id);
    }
}
