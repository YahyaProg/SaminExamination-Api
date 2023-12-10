using Microsoft.EntityFrameworkCore;
using SaminExamination.Context;
using SaminExamination.Interfaces;
using SaminExamination.Models;
using System.Threading;

namespace SaminExamination.Repository
{
    public class CategoryRepostory : ICategoryRepository
    {
        public DataContext _context;
        public CategoryRepostory(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CategoryExist(int ctegoryId)
        {
            return await  _context.categories.AnyAsync(c => c.Id == ctegoryId );
        }

        public async Task<bool> CreateCategory(Category category , CancellationToken cancellationToken)
        {
           await _context.categories.AddAsync(category , cancellationToken);
            return await SaveAsync();
        }

        public async Task <bool> DeleteCategory(Category category)
        {
            _context.Remove(category);
            return await SaveAsync();
        }

        public async Task<ICollection<Category>> GetCategories(CancellationToken cancellationToken)
        {
           return await _context.categories.ToListAsync(cancellationToken);
        }

        public async Task<Category> GetCategory(int id)
        {
            return await _context.categories.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool>  UpdateCategory(Category category ,CancellationToken cancellation)
        {
            _context.Update(category);
            return await SaveAsync();
        }
        public Task<bool>  IsExist(int categoryId)
        {
            return _context.categories.AnyAsync(c => categoryId == categoryId);
        }
    }
}
