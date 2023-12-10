using SaminExamination.Models;

namespace SaminExamination.Interfaces
{
    public interface ICategoryRepository
    {
        Task<ICollection<Category>> GetCategories(CancellationToken cancellationToken);
       Task<Category> GetCategory(int id);
        Task<bool> CreateCategory(Category category , CancellationToken cancellation);
        Task<bool> UpdateCategory(Category category , CancellationToken cancellation);
       Task<bool>  DeleteCategory(Category category);
        Task<bool> SaveAsync();
         Task<bool> CategoryExist(int ctegoryId);
        Task<bool> IsExist(int categoryId);
    }
}
