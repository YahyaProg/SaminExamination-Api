using SaminExamination.Context;
using SaminExamination.Interfaces;
using SaminExamination.Models;

namespace SaminExamination.Repository
{
    public class CategoryRepostory : ICategoryRepository
    {
        public DataContext _context;
        public CategoryRepostory(DataContext context)
        {
            _context = context;
        }
        public bool CategoryExist(int ctegoryId)
        {
            return _context.categories.Any(c => c.Id == ctegoryId);
        }

        public bool CreateCategory(Category category)
        {
            _context.categories.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
           return _context.categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }
        public bool IsExist(int categoryId)
        {
            return _context.categories.Any(c => categoryId == categoryId);
        }
    }
}
