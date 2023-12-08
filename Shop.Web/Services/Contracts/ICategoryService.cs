using Shop.Web.Models;

namespace Shop.Web.Services.Contracts
{
    public interface ICategoryService
    {
        Task <IEnumerable<Category>> Get();
        Task <Category> GetById(int id);
        Task <Category> Post(Category category);
        Task <Category> Put(Category category);
        Task Delete(int id);

    }
}
