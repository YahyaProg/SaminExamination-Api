using SaminExamination.Models;

namespace SaminExamination.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
        bool CategoryExist(int ctegoryId);
        bool IsExist(int categoryId);
    }
}
